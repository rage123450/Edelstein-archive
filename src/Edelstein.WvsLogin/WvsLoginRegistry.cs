using System.IO;
using Edelstein.Database;
using Edelstein.Provider;
using Edelstein.Provider.Items;
using Edelstein.WvsLogin.Sockets;
using Lamar;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PKG1;

namespace Edelstein.WvsLogin
{
    public class WvsLoginRegistry : ServiceRegistry
    {
        public WvsLoginRegistry()
        {
            For<WvsLoginOptions>().Use(c =>
            {
                var configBuilder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("WvsLogin.example.json")
                    .AddJsonFile("WvsLogin.json", true);
                var config = configBuilder.Build();
                var options = new WvsLoginOptions();

                config.Bind(options);
                return options;
            }).Singleton();

            For<DataContext>().Use(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
                
                optionsBuilder.UseMySql(c.GetInstance<WvsLoginOptions>().ConnectionString);
                return new DataContext(optionsBuilder.Options);
            });
            
            For<PackageCollection>().Use(c =>
            {
                WZReader.InitializeKeys();
                return new PackageCollection(c.GetInstance<WvsLoginOptions>().BaseWZPath);
            }).Singleton();

            For<LazyTemplateManager<ItemTemplate>>().Use<ItemTemplateManager>().Singleton();
            
            For<CenterServerSocketFactory>().Use<CenterServerSocketFactory>();
            For<LoginClientSocketFactory>().Use<LoginClientSocketFactory>();
            
            For<WvsLogin>().Use<WvsLogin>().Singleton();
        }
    }
}