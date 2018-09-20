using DotNetty.Transport.Channels;
using Edelstein.Network;
using Edelstein.Network.Packets;
using Lamar;

namespace Edelstein.WvsGame.Sockets
{
    public class CenterServerSocket : Socket
    {
        public CenterServerSocket(IContainer container, IChannel channel, uint seqSend, uint seqRecv)
            : base(channel, seqSend, seqRecv)
        {
        }

        public override void OnPacket(InPacket packet)
        {
        }
    }
}