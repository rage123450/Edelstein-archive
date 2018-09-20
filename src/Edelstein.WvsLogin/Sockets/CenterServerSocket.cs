using System;
using DotNetty.Transport.Channels;
using Edelstein.Network;
using Edelstein.Network.Interop;
using Edelstein.Network.Interop.Game;
using Edelstein.Network.Packets;
using Edelstein.WvsLogin.Logging;
using Lamar;

namespace Edelstein.WvsLogin.Sockets
{
    public class CenterServerSocket : Socket
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

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
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnRegisterServerResult(InPacket packet)
        {
            if (packet.Decode<byte>() > 0) return; // TODO: disconnect?

            var worldInformation = new WorldInformation();

            worldInformation.Decode(packet);
            Logger.Info($"Registered Center server, {worldInformation.Name}");
        }
    }
}