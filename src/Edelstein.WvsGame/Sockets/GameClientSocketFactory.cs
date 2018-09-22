using DotNetty.Transport.Channels;
using Edelstein.Network;
using Lamar;

namespace Edelstein.WvsGame.Sockets
{
    public class GameClientSocketFactory : ISocketFactory<GameClientSocket>
    {
        private readonly IContainer _container;

        public GameClientSocketFactory(IContainer container)
        {
            this._container = container;
        }

        public GameClientSocket Build(IChannel channel, uint seqSend, uint seqRecv)
        {
            return new GameClientSocket(this._container, channel, seqSend, seqRecv);
        }
    }
}