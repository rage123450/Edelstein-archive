using System.Threading.Tasks;
using Edelstein.Network;
using Edelstein.WvsLogin.Interop;

namespace Edelstein.WvsLogin
{
    public class WvsLogin
    {
        private WvsLoginOptions _options;
        private Client<CenterServerSocket> _interopClient;
        private Server<LoginClientSocket> _gameServer;

        public WvsLogin(WvsLoginOptions options)
        {
            this._options = options;
        }

        public async Task Run()
        {
            this._interopClient = new Client<CenterServerSocket>(
                this._options.InteropClientOptions,
                new CenterServerSocketFactory()
            );
            this._gameServer = new Server<LoginClientSocket>(
                this._options.GameServerOptions,
                new LoginClientSocketFactory()
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