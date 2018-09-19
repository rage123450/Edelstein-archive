using System.Threading.Tasks;
using Edelstein.Network;
using Edelstein.WvsCenter.Logging;
using Lamar;

namespace Edelstein.WvsCenter
{
    public class WvsCenter
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private readonly IContainer _container;
        public Server<CenterClientSocket> InteropServer;

        public WvsCenter(IContainer container)
        {
            this._container = container;
        }

        public async Task Run()
        {
            this.InteropServer = new Server<CenterClientSocket>(
                this._container.GetInstance<WvsCenterOptions>().InteropServerOptions,
                this._container.GetInstance<CenterClientSocketFactory>()
            );

            await this.InteropServer.Run();
            Logger.Info($"Bounded WvsCenter on {this.InteropServer.Channel.LocalAddress}");

            await this.InteropServer.Channel.CloseCompletion;
        }
    }
}