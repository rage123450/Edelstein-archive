using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Common.Interop;
using Edelstein.Common.Interop.Game;
using Edelstein.Network;
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
        public List<Client<CenterServerSocket>> InteropClients;
        public Server<LoginClientSocket> GameServer;

        public LoginInformation LoginInformation { get; set; }

        public WvsLogin(IContainer container)
        {
            this._container = container;
            this.InteropClients = new List<Client<CenterServerSocket>>();
        }

        public async Task Run()
        {
            var options = this._container.GetInstance<WvsLoginOptions>();
            var info = options.LoginInfo;

            this.LoginInformation = new LoginInformation
            {
                Name = info.Name
            };

            foreach (var clientOptions in options.InteropClientOptions)
            {
                var client = new Client<CenterServerSocket>(
                    clientOptions,
                    this._container.GetInstance<CenterServerSocketFactory>()
                );

                this.InteropClients.Add(client);
                await client.Run();
                Logger.Info($"Connected to interoperability server on {client.Channel.RemoteAddress}");
            }

            this.GameServer = new Server<LoginClientSocket>(
                options.GameServerOptions,
                this._container.GetInstance<LoginClientSocketFactory>()
            );

            await this.GameServer.Run();
            Logger.Info($"Bounded {this.LoginInformation.Name} on {this.GameServer.Channel.LocalAddress}");
            
            InteropClients.ForEach(c =>
            {
                using (var p = new OutPacket(InteropRecvOperations.RegisterServer))
                {
                    p.Encode<byte>((byte) ServerType.Login);
                    LoginInformation.Encode(p);

                    c.Socket.SendPacket(p);
                }
            });
        }
    }
}