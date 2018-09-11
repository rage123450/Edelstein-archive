using System.Threading.Tasks;
using Edelstein.Network;

namespace Edelstein.WvsCenter
{
    public class WvsCenter
    {
        private WvsCenterOptions _options;
        private Server<CenterClientSocket> _server;

        public WvsCenter(WvsCenterOptions options)
        {
            this._options = options;
        }

        public async Task Run()
        {
            this._server = new Server<CenterClientSocket>(
                this._options.InteropServerOptions,
                new CenterClientSocketFactory()
            );

            await this._server.Run();
            await this._server.Channel.CloseCompletion;
        }
    }
}