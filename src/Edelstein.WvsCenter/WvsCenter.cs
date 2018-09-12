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
        private Server<Socket> _interopServer;

        public WvsCenter(IContainer container)
        {
            this._container = container;
        }

        public async Task Run()
        {
            this._interopServer = new Server<Socket>(
                this._container.GetInstance<WvsCenterOptions>().InteropServerOptions,
                this._container.GetInstance(typeof(ISocketFactory<>))
                    as ISocketFactory<Socket>
            );

            await this._interopServer.Run();
            Logger.Info($"Bounded WvsCenter on {this._interopServer.Channel.LocalAddress}");

            await this._interopServer.Channel.CloseCompletion;
        }
    }
}