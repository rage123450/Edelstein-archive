using System;
using DotNetty.Transport.Channels;
using Edelstein.Common.Interop;
using Edelstein.Common.Interop.Game;
using Edelstein.Network;
using Edelstein.Network.Packets;
using Edelstein.WvsGame.Logging;
using Lamar;

namespace Edelstein.WvsGame.Sockets
{
    public class CenterServerSocket : Socket
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private IContainer _container;
        private WvsGame _wvsGame;

        public CenterServerSocket(IContainer container, IChannel channel, uint seqSend, uint seqRecv)
            : base(channel, seqSend, seqRecv)
        {
            this._container = container;
            this._wvsGame = container.GetInstance<WvsGame>();
        }

        public override void OnPacket(InPacket packet)
        {
            var operation = (InteropSendOperations) packet.Decode<short>();

            switch (operation)
            {
                case InteropSendOperations.ServerRegisterResult:
                    this.OnServerRegisterResult(packet);
                    break;
                case InteropSendOperations.ServerInformation:
                    this.OnServerInformation(packet);
                    break;
                case InteropSendOperations.MigrationRegistryRequest:
                    this.OnMigrationRegistryRequest(packet);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void OnDisconnect()
        {
            throw new NotImplementedException();
        }

        private void OnServerRegisterResult(InPacket packet)
        {
            if (packet.Decode<bool>()) return; // TODO: disconnect?

            var worldInformation = new WorldInformation();

            worldInformation.Decode(packet);
            Logger.Info($"Registered Center server, {worldInformation.Name}");
        }

        private void OnServerInformation(InPacket packet)
        {
            var worldInformation = new WorldInformation();

            worldInformation.Decode(packet);
            Logger.Info($"Updated {worldInformation.Name} server information");
        }

        private void OnMigrationRegistryRequest(InPacket packet)
        {
            using (var p = new OutPacket(InteropRecvOperations.MigrationRegisterResult))
            {
                p.Encode<string>(packet.Decode<string>());
                p.Encode<string>(packet.Decode<string>());

                var characterID = packet.Decode<int>();

                _wvsGame.PendingMigrations.Add(characterID);
                p.Encode<bool>(true);
                p.Encode<int>(characterID);

                SendPacket(p);
            }
        }
    }
}