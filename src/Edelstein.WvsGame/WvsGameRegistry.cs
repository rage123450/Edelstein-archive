using System.IO;
using Edelstein.Database;
using Edelstein.Provider;
using Edelstein.Provider.Fields;
using Edelstein.Provider.Items;
using Edelstein.Provider.Mobs;
using Edelstein.Provider.NPC;
using Edelstein.WvsGame.Commands;
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

            For<ITemplateManager<ItemTemplate>>().Use<ItemTemplateManager>().Singleton();
            For<ITemplateManager<FieldTemplate>>().Use<FieldTemplateManager>().Singleton();
            For<ITemplateManager<NPCTemplate>>().Use<NPCTemplateManager>().Singleton();
            For<ITemplateManager<MobTemplate>>().Use<MobTemplateManager>().Singleton();

            For<CenterServerSocketFactory>().Use<CenterServerSocketFactory>();
            For<GameClientSocketFactory>().Use<GameClientSocketFactory>();

            For<WvsGame>().Use<WvsGame>().Singleton();
        }
    }
}