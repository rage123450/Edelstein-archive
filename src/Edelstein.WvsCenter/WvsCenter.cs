using System.Threading.Tasks;
using Edelstein.Common.Interop.Game;
using Edelstein.Network;
using Edelstein.WvsCenter.Logging;
using Edelstein.WvsCenter.Sockets;
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
            _container = container;
        }

        public async Task Run()
        {
            var options = _container.GetInstance<WvsCenterOptions>();
            var info = options.CenterInfo;

            WorldInformation = new WorldInformation
            {
                ID = info.ID,
                Name = info.Name,
                State = info.State,
                EventDesc = info.EventDesc,
                EventEXP = info.EventEXP,
                EventDrop = info.EventDrop,
                BlockCharCreation = info.BlockCharCreation
            };

            InteropServer = new Server<CenterClientSocket>(
                options.InteropServerOptions,
                _container.GetInstance<CenterClientSocketFactory>()
            );

            await InteropServer.Run();
            Logger.Info($"Bounded {WorldInformation.Name} on {InteropServer.Channel.LocalAddress}");
        }
    }
}