using DotNetty.Transport.Channels;
using Edelstein.Network;

namespace Edelstein.WvsCenter
{
    public class CenterClientSocketFactory : ISocketFactory<CenterClientSocket>
    {
        public CenterClientSocket createNew(IChannel channel, uint seqSend, uint seqRecv)
        {
            return new CenterClientSocket(channel, seqSend, seqRecv);
        }
    }
}