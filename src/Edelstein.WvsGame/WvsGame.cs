using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Common.Interop;
using Edelstein.Common.Interop.Game;
using Edelstein.Network;
using Edelstein.Network.Packets;
using Edelstein.Provider;
using Edelstein.Provider.Fields;
using Edelstein.Provider.NPC;
using Edelstein.WvsGame.Fields;
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
        public Server<GameClientSocket> GameServer;

        public ChannelInformation ChannelInformation { get; set; }
        public ICollection<int> PendingMigrations { get; set; }

        public ITemplateManager<FieldTemplate> FieldTemplates { get; set; }
        public ITemplateManager<NPCTemplate> NPCTemplates { get; set; }
        public FieldFactory FieldFactory { get; set; }

        public WvsGame(IContainer container)
        {
            _container = container;
            PendingMigrations = new List<int>();
        }

        public async Task Run()
        {
            var options = _container.GetInstance<WvsGameOptions>();
            var info = options.GameInfo;

            ChannelInformation = new ChannelInformation
            {
                ID = info.ID,
                WorldID = info.WorldID,
                Name = info.Name,
                UserNo = 0,
                AdultChannel = info.AdultChannel
            };

            FieldTemplates = _container.GetInstance<ITemplateManager<FieldTemplate>>();
            NPCTemplates = _container.GetInstance<ITemplateManager<NPCTemplate>>();
            FieldFactory = new FieldFactory(FieldTemplates, NPCTemplates);

            InteropClient = new Client<CenterServerSocket>(
                options.InteropClientOptions,
                _container.GetInstance<CenterServerSocketFactory>()
            );

            GameServer = new Server<GameClientSocket>(
                options.GameServerOptions,
                _container.GetInstance<GameClientSocketFactory>()
            );

            await InteropClient.Run();
            Logger.Info($"Connected to interoperability server on {InteropClient.Channel.RemoteAddress}");

            await GameServer.Run();
            Logger.Info($"Bounded {ChannelInformation.Name} on {GameServer.Channel.LocalAddress}");

            while (InteropClient.Socket == null) ;

            using (var p = new OutPacket(InteropRecvOperations.ServerRegister))
            {
                p.Encode<byte>((byte) ServerType.Game);
                ChannelInformation.Encode(p);

                await InteropClient.Socket.SendPacket(p);
            }
        }
    }
}