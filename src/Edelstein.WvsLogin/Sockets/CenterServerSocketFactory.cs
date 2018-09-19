using DotNetty.Transport.Channels;
using Edelstein.Network;

namespace Edelstein.WvsLogin.Sockets
{
    public class CenterServerSocketFactory : ISocketFactory<CenterServerSocket>
    {
        public CenterServerSocket Build(IChannel channel, uint seqSend, uint seqRecv)
        {
            return new CenterServerSocket(channel, seqSend, seqRecv);
        }
    }
}