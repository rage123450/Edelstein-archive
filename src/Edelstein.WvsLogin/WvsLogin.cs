using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Common.Interop;
using Edelstein.Common.Interop.Game;
using Edelstein.Network;
using Edelstein.Network.Packets;
using Edelstein.Provider;
using Edelstein.Provider.Items;
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
        
        public LazyTemplateManager<ItemTemplate> ItemTemplates { get; set; }

        public WvsLogin(IContainer container)
        {
            _container = container;
            InteropClients = new List<Client<CenterServerSocket>>();
        }

        public async Task Run()
        {
            var options = _container.GetInstance<WvsLoginOptions>();
            var info = options.LoginInfo;

            LoginInformation = new LoginInformation
            {
                Name = info.Name
            };

            ItemTemplates = _container.GetInstance<LazyTemplateManager<ItemTemplate>>();
            
            foreach (var clientOptions in options.InteropClientOptions)
            {
                var client = new Client<CenterServerSocket>(
                    clientOptions,
                    _container.GetInstance<CenterServerSocketFactory>()
                );

                InteropClients.Add(client);
                await client.Run();
                Logger.Info($"Connected to interoperability server on {client.Channel.RemoteAddress}");
            }

            GameServer = new Server<LoginClientSocket>(
                options.GameServerOptions,
                _container.GetInstance<LoginClientSocketFactory>()
            );

            await GameServer.Run();
            Logger.Info($"Bounded {LoginInformation.Name} on {GameServer.Channel.LocalAddress}");
            
            InteropClients.ForEach(c =>
            {
                using (var p = new OutPacket(InteropRecvOperations.ServerRegister))
                {
                    p.Encode<byte>((byte) ServerType.Login);
                    LoginInformation.Encode(p);

                    c.Socket.SendPacket(p);
                }
            });
        }
    }
}