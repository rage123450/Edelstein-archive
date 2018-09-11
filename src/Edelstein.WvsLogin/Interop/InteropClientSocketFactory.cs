using DotNetty.Transport.Channels;
using Edelstein.Network;

namespace Edelstein.WvsLogin.Interop
{
    public class InteropClientSocketFactory : ISocketFactory<InteropClientSocket>
    {
        public InteropClientSocket createNew(IChannel channel, uint seqSend, uint seqRecv)
        {
            return new InteropClientSocket(channel, seqSend, seqRecv);
        }
    }
}