using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Common.Interop;
using Edelstein.Common.Interop.Game;
using Edelstein.Network;
using Edelstein.Network.Packets;
using Edelstein.Provider;
using Edelstein.Provider.Fields;
using Edelstein.WvsGame.Fields;
using Edelstein.WvsGame.Logging;
using Edelstein.WvsGame.Sockets;
using Lamar;
using MoreLinq.Extensions;
using PKG1;

namespace Edelstein.WvsGame
{
    public class WvsGame
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        private readonly IContainer _container;
        public Client<CenterServerSocket> InteropClient;
        public Server<GameClientSocket> GameServer;

        public ChannelInformation ChannelInformation { get; set; }
        public ICollection<int> PendingMigrations { get; set; }

        public ITemplateManager<FieldTemplate> FieldTemplates { get; set; }
        public FieldFactory FieldFactory { get; set; }

        public WvsGame(IContainer container)
        {
            this._container = container;
            this.PendingMigrations = new List<int>();
        }

        public async Task Run()
        {
            var options = this._container.GetInstance<WvsGameOptions>();
            var info = options.GameInfo;

            this.ChannelInformation = new ChannelInformation
            {
                ID = info.ID,
                WorldID = info.WorldID,
                Name = info.Name,
                UserNo = 0,
                AdultChannel = info.AdultChannel
            };

            this.FieldTemplates = this._container.GetInstance<ITemplateManager<FieldTemplate>>();
            this.FieldFactory = new FieldFactory(FieldTemplates);
            
            this.InteropClient = new Client<CenterServerSocket>(
                options.InteropClientOptions,
                this._container.GetInstance<CenterServerSocketFactory>()
            );

            this.GameServer = new Server<GameClientSocket>(
                options.GameServerOptions,
                this._container.GetInstance<GameClientSocketFactory>()
            );

            await this.InteropClient.Run();
            Logger.Info($"Connected to interoperability server on {this.InteropClient.Channel.RemoteAddress}");

            await this.GameServer.Run();
            Logger.Info($"Bounded {this.ChannelInformation.Name} on {this.GameServer.Channel.LocalAddress}");

            using (var p = new OutPacket(InteropRecvOperations.ServerRegister))
            {
                p.Encode<byte>((byte) ServerType.Game);
                ChannelInformation.Encode(p);

                await this.InteropClient.Socket.SendPacket(p);
            }
        }
    }
}