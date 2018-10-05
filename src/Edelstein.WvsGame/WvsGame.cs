using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Common.Interop;
using Edelstein.Common.Interop.Game;
using Edelstein.Network;
using Edelstein.Network.Packets;
using Edelstein.Provider;
using Edelstein.Provider.Fields;
using Edelstein.Provider.Items;
using Edelstein.Provider.Items.Options;
using Edelstein.Provider.Mobs;
using Edelstein.Provider.NPC;
using Edelstein.Provider.Skills;
using Edelstein.Provider.Strings;
using Edelstein.WvsGame.Commands;
using Edelstein.WvsGame.Conversations;
using Edelstein.WvsGame.Conversations.Speakers;
using Edelstein.WvsGame.Fields;
using Edelstein.WvsGame.Fields.Objects;
using Edelstein.WvsGame.Fields.Objects.Users;
using Edelstein.WvsGame.Logging;
using Edelstein.WvsGame.Sockets;
using Lamar;
using MoonSharp.Interpreter;

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

        public CommandRegistry CommandRegistry { get; set; }

        public ItemNameManager ItemNames { get; set; }
        public FieldNameManager FieldNames { get; set; }

        public LazyTemplateManager<SkillTemplate> SkillTemplates { get; set; }
        public EagerTemplateManager<ItemOptionTemplate> ItemOptions { get; set; }
        public LazyTemplateManager<ItemTemplate> ItemTemplates { get; set; }
        public LazyTemplateManager<FieldTemplate> FieldTemplates { get; set; }
        public LazyTemplateManager<NPCTemplate> NpcTemplates { get; set; }
        public LazyTemplateManager<MobTemplate> MobTemplates { get; set; }
        public FieldFactory FieldFactory { get; set; }

        public ConversationManager<FieldUser, FieldNPC> NPCConversationManager { get; set; }

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

            CommandRegistry = _container.GetInstance<CommandRegistry>();

            ItemNames = _container.GetInstance<ItemNameManager>();
            FieldNames = _container.GetInstance<FieldNameManager>();

            Logger.Info("Loading template names..");
            await Task.WhenAll(
                ItemNames.LoadAll(),
                FieldNames.LoadAll()
            );
            Logger.Info("Finished loading template names");


            ItemOptions = _container.GetInstance<EagerTemplateManager<ItemOptionTemplate>>();
            await Task.WhenAll(
                Task.Run(async () =>
                {
                    Logger.Info("Loading item options..");
                    await ItemOptions.LoadAll();
                    Logger.Info("Finished loading item options");
                })
            );

            SkillTemplates = _container.GetInstance<LazyTemplateManager<SkillTemplate>>();
            ItemTemplates = _container.GetInstance<LazyTemplateManager<ItemTemplate>>();
            FieldTemplates = _container.GetInstance<LazyTemplateManager<FieldTemplate>>();
            NpcTemplates = _container.GetInstance<LazyTemplateManager<NPCTemplate>>();
            MobTemplates = _container.GetInstance<LazyTemplateManager<MobTemplate>>();
            FieldFactory = new FieldFactory(FieldTemplates, NpcTemplates, MobTemplates);

            UserData.RegisterType<FieldUserSpeaker>();
            UserData.RegisterType<FieldNPCSpeaker>();
            NPCConversationManager = _container.GetInstance<ConversationManager<FieldUser, FieldNPC>>();

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