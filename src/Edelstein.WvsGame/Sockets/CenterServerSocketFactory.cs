using DotNetty.Transport.Channels;
using Edelstein.Network;
using Lamar;

namespace Edelstein.WvsGame.Sockets
{
    public class CenterServerSocketFactory : ISocketFactory<CenterServerSocket>
    {
        private readonly IContainer _container;

        public CenterServerSocketFactory(IContainer container)
        {
            _container = container;
        }

        public CenterServerSocket Build(IChannel channel, uint seqSend, uint seqRecv)
        {
            return new CenterServerSocket(_container, channel, seqSend, seqRecv);
        }
    }
}