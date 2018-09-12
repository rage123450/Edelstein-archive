using System.Threading.Tasks;
using Edelstein.Network;
using Lamar;

namespace Edelstein.WvsCenter
{
    public class WvsCenter
    {
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
            await this._interopServer.Channel.CloseCompletion;
        }
    }
}