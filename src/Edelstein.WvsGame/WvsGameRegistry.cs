using System.IO;
using Edelstein.Database;
using Edelstein.WvsGame.Sockets;
using Lamar;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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

            For<CenterServerSocketFactory>().Use<CenterServerSocketFactory>();

            For<WvsGame>().Use<WvsGame>().Singleton();
        }
    }
}