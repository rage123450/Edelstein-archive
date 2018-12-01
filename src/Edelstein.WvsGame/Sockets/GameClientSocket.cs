using System.Linq;
using DotNetty.Transport.Channels;
using Edelstein.Common.Utils;
using Edelstein.Database;
using Edelstein.Database.Entities;
using Edelstein.Database.Entities.Types;
using Edelstein.Network;
using Edelstein.Network.Packets;
using Edelstein.Provider.Fields;
using Edelstein.WvsGame.Fields.Objects.Users;
using Edelstein.WvsGame.Logging;
using Edelstein.WvsGame.Packets;
using Lamar;
using Microsoft.EntityFrameworkCore;
using MoreLinq;

namespace Edelstein.WvsGame.Sockets
{
    public class GameClientSocket : Socket
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private IContainer _container;
        public WvsGame WvsGame { get; }

        public Rand32 Random { get; set; }
        public FieldUser FieldUser { get; set; }
        public bool IsInstantiated { get; set; }

        public GameClientSocket(IContainer container, IChannel channel, uint seqSend, uint seqRecv)
            : base(channel, seqSend, seqRecv)
        {
            _container = container;
            WvsGame = container.GetInstance<WvsGame>();
        }

        public override void OnPacket(InPacket packet)
        {
            var operation = (GameRecvOperations) packet.Decode<short>();

            switch (operation)
            {
                case GameRecvOperations.MigrateIn:
                    OnMigrateIn(packet);
                    break;
                case GameRecvOperations.FuncKeyMappedModified:
                    OnFuncKeyMappedModified(packet);
                    break;
                case GameRecvOperations.QuickslotKeyMappedModified:
                    OnQuickslotKeyMappedModified(packet);
                    break;
                default:
                    if (!FieldUser?.Field.OnPacket(FieldUser, operation, packet) ?? false)
                        Logger.Warn($"Unhandled packet operation {operation}");
                    break;
            }
        }

        private void OnMigrateIn(InPacket packet)
        {
            var characterID = packet.Decode<int>();

            if (!WvsGame.PendingMigrations.Remove(characterID))
            {
                Channel.CloseAsync();
                return;
            }

            using (var db = _container.GetInstance<DataContext>())
            {
                var character = db.Characters
                    .Include(c => c.Data)
                    .ThenInclude(a => a.Account)
                    .Include(c => c.FunctionKeys)
                    .Include(c => c.QuickslotKeys)
                    .Include(c => c.Macros)
                    .Include(c => c.Inventories)
                    .ThenInclude(c => c.Items)
                    .Include(c => c.SkillRecords)
                    .Single(c => c.ID == characterID);

                character.Data.Account.State = AccountState.LoggedIn;
                db.Update(character);
                db.SaveChanges();

                var field = WvsGame.FieldFactory.Get(character.FieldID);
                var fieldUser = new FieldUser(this, character);

                Random = new Rand32(0x0, 0x0, 0x0);
                FieldUser = fieldUser;
                field.Enter(fieldUser);

                using (var p = new OutPacket(GameSendOperations.FuncKeyMappedInit))
                {
                    var functionKeys = character.FunctionKeys;

                    p.Encode<bool>(false);
                    for (var i = 0; i < 90; i++)
                    {
                        var functionKey = functionKeys.SingleOrDefault(f => f.Key == i);

                        p.Encode<byte>(functionKey?.Type ?? 0);
                        p.Encode<int>(functionKey?.Action ?? 0);
                    }

                    SendPacket(p);
                }

                using (var p = new OutPacket(GameSendOperations.QuickslotMappedInit))
                {
                    var quickslots = character.QuickslotKeys.ToList();
                    var notDefault = quickslots.Count > 0;

                    p.Encode<bool>(notDefault);
                    if (notDefault)
                    {
                        for (var i = 0; i < 8; i++)
                            p.Encode<int>(quickslots.ElementAtOrDefault(i)?.Key ?? 0);
                    }

                    SendPacket(p);
                }

                using (var p = new OutPacket(GameSendOperations.MacroSysDataInit))
                {
                    p.Encode<byte>((byte) character.Macros.Count);
                    character.Macros.ForEach(m =>
                    {
                        p.Encode<string>(m.Name);
                        p.Encode<bool>(m.Mute);
                        p.Encode<int>(m.Skill1);
                        p.Encode<int>(m.Skill2);
                        p.Encode<int>(m.Skill3);
                    });
                    SendPacket(p);
                }
            }
        }

        private void OnFuncKeyMappedModified(InPacket packet)
        {
            var v3 = packet.Decode<int>();

            if (v3 > 0) return;
            var count = packet.Decode<int>();

            for (var i = 0; i < count; i++)
            {
                var key = packet.Decode<int>();
                var type = packet.Decode<byte>();
                var action = packet.Decode<int>();

                var functionKeys = FieldUser.Character.FunctionKeys;
                var functionKey = functionKeys.SingleOrDefault(f => f.Key == key);

                if (type > 0 || action > 0)
                {
                    if (functionKey != null)
                    {
                        functionKey.Type = type;
                        functionKey.Action = action;
                    }
                    else
                    {
                        functionKeys.Add(new FunctionKey
                        {
                            Key = key,
                            Type = type,
                            Action = action
                        });
                    }
                }
                else functionKeys.Remove(functionKey);
            }
        }

        private void OnQuickslotKeyMappedModified(InPacket packet)
        {
            var quickslots = FieldUser.Character.QuickslotKeys.ToList();
            for (var i = 0; i < 8; i++)
            {
                var quickslot = quickslots.ElementAtOrDefault(i) ?? new QuickslotKey();

                quickslot.Key = packet.Decode<int>();
                if (!quickslots.Contains(quickslot)) FieldUser.Character.QuickslotKeys.Add(quickslot);
            }
        }

        public override void OnDisconnect()
        {
            var u = FieldUser;

            if (u != null)
            {
                using (var db = _container.GetInstance<DataContext>())
                {
                    var account = u.Character.Data.Account;

                    if (account.State != AccountState.MigratingIn)
                        account.State = AccountState.LoggedOut;

                    var character = u.Character;

                    character.FieldPortal = (byte) u.Field.Template.Portals
                        .Values
                        .Where(p => p.Type == FieldPortalType.Spawn)
                        .OrderBy(p =>
                        {
                            var xd = p.Position.X - u.Position.X;
                            var yd = p.Position.Y - u.Position.Y;

                            return xd * xd + yd * yd;
                        })
                        .First()
                        .ID;

                    db.Update(character);
                    db.SaveChanges();
                }

                u.ConversationContext?.TokenSource.Cancel();
                u.TemporaryStatTimers.Values.ForEach(t => t.Stop());
                u.Field?.Leave(u);
            }
        }
    }
}