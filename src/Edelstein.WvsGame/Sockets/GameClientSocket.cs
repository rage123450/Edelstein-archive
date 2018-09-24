using System;
using System.Linq;
using DotNetty.Transport.Channels;
using Edelstein.Database;
using Edelstein.Network;
using Edelstein.Network.Packets;
using Edelstein.WvsGame.Fields.Users;
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
                    var characterID = packet.Decode<int>();

                    if (!_wvsGame.PendingMigrations.Remove(characterID))
                    {
                        Channel.CloseAsync();
                        return;
                    }

                    using (var db = _container.GetInstance<DataContext>())
                    {
                        var character = db.Characters
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
                        var field = _wvsGame.FieldFactory.Get(character.FieldID);
                        var fieldUser = new FieldUser(this, character);

                        FieldUser = fieldUser;
                        field.Enter(fieldUser);
                    }

                    break;
                default:
                    Logger.Warn($"Unhandled packet operation {operation}");
                    break;
            }
        }

        public override void OnDisconnect()
        {
            var fieldUser = FieldUser;
            fieldUser?.Field?.Leave(fieldUser);
        }
    }
}