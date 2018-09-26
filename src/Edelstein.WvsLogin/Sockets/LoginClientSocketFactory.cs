using DotNetty.Transport.Channels;
using Edelstein.Network;
using Lamar;

namespace Edelstein.WvsLogin.Sockets
{
    public class LoginClientSocketFactory : ISocketFactory<LoginClientSocket>
    {
        private readonly IContainer _container;

        public LoginClientSocketFactory(IContainer container)
        {
            _container = container;
        }

        public LoginClientSocket Build(IChannel channel, uint seqSend, uint seqRecv)
        {
            return new LoginClientSocket(_container, channel, seqSend, seqRecv);
        }
    }
}