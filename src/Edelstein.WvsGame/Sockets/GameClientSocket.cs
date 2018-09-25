using System.Linq;
using DotNetty.Transport.Channels;
using Edelstein.Database;
using Edelstein.Database.Entities.Types;
using Edelstein.Network;
using Edelstein.Network.Packets;
using Edelstein.WvsGame.Fields.Objects;
using Edelstein.WvsGame.Logging;
using Edelstein.WvsGame.Packets;
using Lamar;
using Microsoft.EntityFrameworkCore;

namespace Edelstein.WvsGame.Sockets
{
    public class GameClientSocket : Socket
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private IContainer _container;
        private WvsGame _wvsGame;

        public FieldUser FieldUser { get; set; }
        public bool IsInstantiated { get; set; }

        public GameClientSocket(IContainer container, IChannel channel, uint seqSend, uint seqRecv)
            : base(channel, seqSend, seqRecv)
        {
            this._container = container;
            this._wvsGame = container.GetInstance<WvsGame>();
        }

        public override void OnPacket(InPacket packet)
        {
            var operation = (GameRecvOperations) packet.Decode<short>();

            switch (operation)
            {
                case GameRecvOperations.MigrateIn:
                    this.OnMigrateIn(packet);
                    break;
                default:
                    if (!FieldUser?.OnPacket(operation, packet) ?? false)
                        Logger.Warn($"Unhandled packet operation {operation}");
                    break;
            }
        }

        private void OnMigrateIn(InPacket packet)
        {
            var characterID = packet.Decode<int>();

            if (!_wvsGame.PendingMigrations.Remove(characterID))
            {
                Channel.CloseAsync();
                return;
            }

            using (var db = _container.GetInstance<DataContext>())
            {
                var character = db.Characters
                    .Include(c => c.Account)
                    .Include(c => c.InventoryEquipped)
                    .ThenInclude(c => c.Items)
                    .Include(c => c.InventoryEquippedCash)
                    .ThenInclude(c => c.Items)
                    .Include(c => c.InventoryEquip)
                    .ThenInclude(c => c.Items)
                    .Include(c => c.InventoryConsume)
                    .ThenInclude(c => c.Items)
                    .Include(c => c.InventoryInstall)
                    .ThenInclude(c => c.Items)
                    .Include(c => c.InventoryEtc)
                    .ThenInclude(c => c.Items)
                    .Include(c => c.InventoryCash)
                    .ThenInclude(c => c.Items)
                    .Single(c => c.ID == characterID);

                character.Account.State = AccountState.LoggedIn;
                db.Update(character);
                db.SaveChanges();

                var field = _wvsGame.FieldFactory.Get(character.FieldID);
                var fieldUser = new FieldUser(this, character);

                FieldUser = fieldUser;
                field.Enter(fieldUser);
            }
        }

        public override void OnDisconnect()
        {
            var fieldUser = FieldUser;

            if (fieldUser != null)
            {
                using (var db = _container.GetInstance<DataContext>())
                {
                    var account = fieldUser.Character.Account;

                    if (account.State != AccountState.MigratingIn)
                        account.State = AccountState.LoggedOut;

                    db.Update(fieldUser.Character);
                    db.SaveChanges();
                }
            }

            fieldUser?.Field?.Leave(fieldUser);
        }
    }
}