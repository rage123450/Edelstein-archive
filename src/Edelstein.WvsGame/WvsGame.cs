using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Interop;
using Edelstein.Common.Interop.Game;
using Edelstein.Database;
using Edelstein.Network;
using Edelstein.Network.Packets;
using Edelstein.Provider;
using Edelstein.Provider.Fields;
using Edelstein.Provider.Items;
using Edelstein.Provider.Items.Options;
using Edelstein.Provider.Items.Set;
using Edelstein.Provider.Mobs;
using Edelstein.Provider.NPC;
using Edelstein.Provider.Reactors;
using Edelstein.Provider.Skills;
using Edelstein.Provider.Strings;
using Edelstein.WvsGame.Commands;
using Edelstein.WvsGame.Conversations;
using Edelstein.WvsGame.Conversations.Speakers;
using Edelstein.WvsGame.Fields;
using Edelstein.WvsGame.Fields.Objects;
using Edelstein.WvsGame.Fields.Objects.Users;
using Edelstein.WvsGame.Interactions.Dialogue;
using Edelstein.WvsGame.Logging;
using Edelstein.WvsGame.Sockets;
using Lamar;
using Microsoft.EntityFrameworkCore;
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
        public EagerTemplateManager<SetItemInfoTemplate> SetItemInfo { get; set; }
        public LazyTemplateManager<ItemTemplate> ItemTemplates { get; set; }
        public LazyTemplateManager<FieldTemplate> FieldTemplates { get; set; }
        public LazyTemplateManager<NPCTemplate> NpcTemplates { get; set; }
        public LazyTemplateManager<MobTemplate> MobTemplates { get; set; }
        public LazyTemplateManager<ReactorTemplate> ReactorTemplates { get; set; }
        public FieldFactory FieldFactory { get; set; }

        public IDictionary<int, NPCShopDlg> NPCShops { get; set; }

        public ConversationManager<FieldUser, FieldNPC> NPCConversationManager { get; set; }
        public ConversationManager<FieldUser, FieldReactor> ReactorConversationManager { get; set; }
        public ConversationManager<FieldUser, FieldPortalTemplate> PortalConversationManager { get; set; }

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
            SetItemInfo = _container.GetInstance<EagerTemplateManager<SetItemInfoTemplate>>();
            await Task.WhenAll(
                Task.Run(async () =>
                {
                    Logger.Info("Loading item options..");
                    await ItemOptions.LoadAll();
                    Logger.Info("Finished loading item options");
                }),
                Task.Run(async () =>
                {
                    Logger.Info("Loading set item info..");
                    await SetItemInfo.LoadAll();
                    Logger.Info("Finished loading set item info");
                })
            );

            SkillTemplates = _container.GetInstance<LazyTemplateManager<SkillTemplate>>();
            ItemTemplates = _container.GetInstance<LazyTemplateManager<ItemTemplate>>();
            FieldTemplates = _container.GetInstance<LazyTemplateManager<FieldTemplate>>();
            NpcTemplates = _container.GetInstance<LazyTemplateManager<NPCTemplate>>();
            MobTemplates = _container.GetInstance<LazyTemplateManager<MobTemplate>>();
            ReactorTemplates = _container.GetInstance<LazyTemplateManager<ReactorTemplate>>();
            FieldFactory = new FieldFactory(FieldTemplates, NpcTemplates, MobTemplates, ReactorTemplates);

            using (var db = _container.GetInstance<DataContext>())
            {
                Logger.Info("Loading npc shops..");
                NPCShops = db.NPCShops
                    .Include(s => s.Items)
                    .ToDictionary(s => s.TemplateID, s => new NPCShopDlg(s));
                Logger.Info("Finished loading npc shops");
            }

            UserData.RegisterType<FieldUserSpeaker>();
            UserData.RegisterType<FieldNPCSpeaker>();
            UserData.RegisterType<NPCSpeaker>();
            NPCConversationManager = _container.GetInstance<ConversationManager<FieldUser, FieldNPC>>();
            ReactorConversationManager = _container.GetInstance<ConversationManager<FieldUser, FieldReactor>>();
            PortalConversationManager = _container.GetInstance<ConversationManager<FieldUser, FieldPortalTemplate>>();

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