using System.Threading.Tasks;
using Edelstein.Network;

namespace Edelstein.WvsLogin
{
    public class WvsLogin
    {
        private WvsLoginOptions _options;
        private Server<LoginClientSocket> _server;

        public WvsLogin(WvsLoginOptions options)
        {
            this._options = options;
        }

        public async Task Run()
        {
            this._server = new Server<LoginClientSocket>(
                this._options.GameServerOptions,
                new LoginClientSocketFactory()
            );

            await this._server.Run();
            await this._server.Channel.CloseCompletion;
        }
    }
}