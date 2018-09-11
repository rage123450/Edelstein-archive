using DotNetty.Transport.Channels;
using Edelstein.Network;
using Edelstein.Network.Packets;

namespace Edelstein.WvsLogin.Interop
{
    public class InteropClientSocket : Socket
    {
        public InteropClientSocket(IChannel channel, uint seqSend, uint seqRecv)
            : base(channel, seqSend, seqRecv)
        {
        }

        public override void OnPacket(InPacket packet)
        {
        }
    }
}