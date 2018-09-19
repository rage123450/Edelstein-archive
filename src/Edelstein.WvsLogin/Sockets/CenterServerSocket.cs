using DotNetty.Transport.Channels;
using Edelstein.Network;
using Edelstein.Network.Packets;

namespace Edelstein.WvsLogin.Sockets
{
    public class CenterServerSocket : Socket
    {
        public CenterServerSocket(IChannel channel, uint seqSend, uint seqRecv)
            : base(channel, seqSend, seqRecv)
        {
        }

        public override void OnPacket(InPacket packet)
        {
        }
    }
}