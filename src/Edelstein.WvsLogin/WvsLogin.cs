using System.Threading.Tasks;
using Edelstein.Network;
using Edelstein.Network.Interop;
using Edelstein.Network.Interop.Game;
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

        public LoginInformation LoginInformation { get; set; }

        public WvsLogin(IContainer container)
        {
            this._container = container;
        }

        public async Task Run()
        {
            var options = this._container.GetInstance<WvsLoginOptions>();
            var info = options.LoginInfo;

            this.LoginInformation = new LoginInformation
            {
                Name = info.Name
            };

            this.InteropClient = new Client<CenterServerSocket>(
                options.InteropClientOptions,
                this._container.GetInstance<CenterServerSocketFactory>()
            );
            this.GameServer = new Server<LoginClientSocket>(
                options.GameServerOptions,
                this._container.GetInstance<LoginClientSocketFactory>()
            );

            await this.InteropClient.Run();
            Logger.Info($"Connected to interoperability server on {this.InteropClient.Channel.RemoteAddress}");

            await this.GameServer.Run();
            Logger.Info($"Bounded {this.LoginInformation.Name} on {this.GameServer.Channel.LocalAddress}");

            using (var p = new OutPacket(InteropRecvOperations.RegisterServer))
            {
                p.Encode<byte>((byte) ServerType.Login);
                LoginInformation.Encode(p);

                await this.InteropClient.Socket.SendPacket(p);
            }
        }
    }
}