using System.IO;
using Edelstein.Database;
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
using Edelstein.WvsGame.Fields.Objects;
using Edelstein.WvsGame.Fields.Objects.Users;
using Edelstein.WvsGame.Sockets;
using Lamar;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PKG1;

namespace Edelstein.WvsGame
{
    public class WvsGameRegistry : ServiceRegistry
    {
        public WvsGameRegistry()
        {
            For<WvsGameOptions>().Use(c =>
            {
                var configBuilder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("WvsGame.example.json")
                    .AddJsonFile("WvsGame.json", true);
                var config = configBuilder.Build();
                var options = new WvsGameOptions();

                config.Bind(options);
                return options;
            }).Singleton();

            For<DataContext>().Use(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

                optionsBuilder.UseMySql(c.GetInstance<WvsGameOptions>().ConnectionString);
                return new DataContext(optionsBuilder.Options);
            });

            For<CommandRegistry>().Use<CommandRegistry>().Singleton();

            For<PackageCollection>().Use(c =>
            {
                WZReader.InitializeKeys();
                return new PackageCollection(c.GetInstance<WvsGameOptions>().BaseWZPath);
            }).Singleton();

            For<ItemNameManager>().Use<ItemNameManager>().Singleton();
            For<FieldNameManager>().Use<FieldNameManager>().Singleton();
            For<EagerTemplateManager<ItemOptionTemplate>>().Use<ItemOptionTemplateManager>().Singleton();
            For<LazyTemplateManager<SkillTemplate>>().Use<SkillTemplateManager>().Singleton();
            For<LazyTemplateManager<ItemTemplate>>().Use<ItemLazyTemplateManager>().Singleton();
            For<LazyTemplateManager<FieldTemplate>>().Use<FieldLazyTemplateManager>().Singleton();
            For<LazyTemplateManager<NPCTemplate>>().Use<NpcLazyTemplateManager>().Singleton();
            For<LazyTemplateManager<MobTemplate>>().Use<MobLazyTemplateManager>().Singleton();

            For<ConversationManager<FieldUser, FieldNPC>>().Use<NPCConversationManager>().Singleton();

            For<CenterServerSocketFactory>().Use<CenterServerSocketFactory>();
            For<GameClientSocketFactory>().Use<GameClientSocketFactory>();

            For<WvsGame>().Use<WvsGame>().Singleton();
        }
    }
}