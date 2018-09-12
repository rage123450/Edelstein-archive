using System.Threading.Tasks;
using Edelstein.Network;
using Edelstein.WvsLogin.Interop;
using Lamar;

namespace Edelstein.WvsLogin
{
    public class WvsLogin
    {
        private readonly IContainer _container;
        private Client<Socket> _interopClient;
        private Server<Socket> _gameServer;

        public WvsLogin(IContainer container)
        {
            this._container = container;
        }

        public async Task Run()
        {
            var options = this._container.GetInstance<WvsLoginOptions>();

            this._interopClient = new Client<Socket>(
                options.InteropClientOptions,
                this._container.GetInstance(typeof(ISocketFactory<>), "Interop")
                    as ISocketFactory<Socket>
            );
            this._gameServer = new Server<Socket>(
                options.GameServerOptions,
                this._container.GetInstance(typeof(ISocketFactory<>), "Game")
                    as ISocketFactory<Socket>
            );

            await this._interopClient.Run();
            await this._gameServer.Run();

            await Task.WhenAll(
                this._interopClient.Channel.CloseCompletion,
                this._gameServer.Channel.CloseCompletion
            );
        }
    }
}