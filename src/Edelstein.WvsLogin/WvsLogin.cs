using System.Threading.Tasks;
using Edelstein.Network;
using Edelstein.Network.Interop;
using Edelstein.Network.Packets;
using Edelstein.WvsLogin.Logging;
using Edelstein.WvsLogin.Sockets;
using Lamar;

namespace Edelstein.WvsLogin
{
    public class WvsLogin
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private readonly IContainer _container;
        public Client<CenterServerSocket> InteropClient;
        public Server<LoginClientSocket> GameServer;

        public WvsLogin(IContainer container)
        {
            this._container = container;
        }

        public async Task Run()
        {
            var options = this._container.GetInstance<WvsLoginOptions>();

            this.InteropClient = new Client<CenterServerSocket>(
                options.InteropClientOptions,
                this._container.GetInstance<CenterServerSocketFactory>()
            );
            this.GameServer = new Server<LoginClientSocket>(
                options.GameServerOptions,
                this._container.GetInstance<LoginClientSocketFactory>()
            );

            await this.InteropClient.Run();
            Logger.Info($"Connected to WvsCenter on {this.InteropClient.Channel.RemoteAddress}");

            await this.GameServer.Run();
            Logger.Info($"Bounded WvsLogin on {this.GameServer.Channel.LocalAddress}");

            using (var p = new OutPacket(InteropRecvOperations.RegisterServer))
            {
                p.Encode<byte>((byte) ServerType.Login);
                p.Encode<string>(options.ServerName);

                await this.InteropClient.Socket.SendPacket(p);
            }
        }
    }
}