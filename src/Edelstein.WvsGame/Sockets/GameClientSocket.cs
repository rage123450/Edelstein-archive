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

        public bool IsInstantiated { get; set; } = false;

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
                        var field = _wvsGame.FieldFactory.Get(100000000);
                        var fieldUser = new FieldUser(this, character);

                        field.Enter(fieldUser);
                    }

                    break;
                case GameRecvOperations.ClientDumpLog:
                    var callType = packet.Decode<short>();
                    var errorCode = packet.Decode<int>();
                    var backupBufferSize = packet.Decode<short>();
                    var rawSeq = packet.Decode<int>();
                    var type = packet.Decode<short>();
                    Console.WriteLine(type.ToString("X"));
                    break;
            }
        }

        public override void OnDisconnect()
        {
            throw new System.NotImplementedException();
        }
    }
}