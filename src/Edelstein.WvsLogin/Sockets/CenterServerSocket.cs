using System;
using DotNetty.Transport.Channels;
using Edelstein.Common.Interop;
using Edelstein.Common.Interop.Game;
using Edelstein.Network;
using Edelstein.Network.Packets;
using Edelstein.WvsLogin.Logging;
using Lamar;

namespace Edelstein.WvsLogin.Sockets
{
    public class CenterServerSocket : Socket
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        
        public WorldInformation WorldInformation { get; set; }

        public CenterServerSocket(IContainer container, IChannel channel, uint seqSend, uint seqRecv)
            : base(channel, seqSend, seqRecv)
        {
        }

        public override void OnPacket(InPacket packet)
        {
            var operation = (InteropSendOperations) packet.Decode<short>();
            
            switch (operation)
            {
                case InteropSendOperations.RegisterServerResult:
                    this.OnRegisterServerResult(packet);
                    break;
                case InteropSendOperations.UpdateWorldInformation:
                    this.OnUpdateWorldInformation(packet);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void OnDisconnect()
        {
            throw new NotImplementedException();
        }

        private void OnRegisterServerResult(InPacket packet)
        {
            if (packet.Decode<bool>()) return; // TODO: disconnect?

            var worldInformation = new WorldInformation();

            worldInformation.Decode(packet);
            this.WorldInformation = worldInformation;
            Logger.Info($"Registered Center server, {worldInformation.Name}");
        }

        private void OnUpdateWorldInformation(InPacket packet)
        {
            var worldInformation = new WorldInformation();

            worldInformation.Decode(packet);
            this.WorldInformation = worldInformation;
            Logger.Info($"Updated {worldInformation.Name} server information");
        }
    }
}