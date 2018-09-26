using DotNetty.Transport.Channels;
using Edelstein.Network;
using Lamar;

namespace Edelstein.WvsCenter.Sockets
{
    public class CenterClientSocketFactory : ISocketFactory<CenterClientSocket>
    {
        private readonly IContainer _container;

        public CenterClientSocketFactory(IContainer container)
        {
            _container = container;
        }

        public CenterClientSocket Build(IChannel channel, uint seqSend, uint seqRecv)
        {
            return new CenterClientSocket(_container, channel, seqSend, seqRecv);
        }
    }
}