using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Network;
using Edelstein.Network.Interop.Game;
using Edelstein.WvsCenter.Logging;
using Lamar;

namespace Edelstein.WvsCenter
{
    public class WvsCenter
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private readonly IContainer _container;
        public Server<CenterClientSocket> InteropServer;

        public WorldInformation WorldInformation { get; set; }

        public WvsCenter(IContainer container)
        {
            this._container = container;
        }

        public async Task Run()
        {
            var options = this._container.GetInstance<WvsCenterOptions>();
            var info = options.CenterInfo;

            this.WorldInformation = new WorldInformation
            {
                ID = info.ID,
                Name = info.Name,
                State = info.State,
                EventDesc = info.EventDesc,
                EventEXP = info.EventEXP,
                EventDrop = info.EventDrop,
                BlockCharCreation = info.BlockCharCreation
            };

            this.InteropServer = new Server<CenterClientSocket>(
                options.InteropServerOptions,
                this._container.GetInstance<CenterClientSocketFactory>()
            );

            await this.InteropServer.Run();
            Logger.Info($"Bounded {this.WorldInformation.Name} on {this.InteropServer.Channel.LocalAddress}");
        }
    }
}