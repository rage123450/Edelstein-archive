using System.Threading.Tasks;
using Edelstein.Network;
using Edelstein.Network.Interop;
using Edelstein.Network.Packets;
using Edelstein.WvsGame.Logging;
using Edelstein.WvsGame.Sockets;
using Lamar;

namespace Edelstein.WvsGame
{
    public class WvsGame
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private readonly IContainer _container;
        public Client<CenterServerSocket> InteropClient;

        public WvsGame(IContainer container)
        {
            this._container = container;
        }

        public async Task Run()
        {
            var options = this._container.GetInstance<WvsGameOptions>();

            this.InteropClient = new Client<CenterServerSocket>(
                options.InteropClientOptions,
                this._container.GetInstance<CenterServerSocketFactory>()
            );

            await this.InteropClient.Run();
            Logger.Info($"Connected to WvsCenter on {this.InteropClient.Channel.RemoteAddress}");

            using (var p = new OutPacket(InteropRecvOperations.RegisterServer))
            {
                p.Encode<byte>((byte) ServerType.Game);
                p.Encode<string>(options.ServerName);

                await this.InteropClient.Socket.SendPacket(p);
            }
        }
    }
}