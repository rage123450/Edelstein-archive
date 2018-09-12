using System.IO;
using Edelstein.Network;
using Lamar;
using Microsoft.Extensions.Configuration;

namespace Edelstein.WvsCenter
{
    public class WvsCenterRegistry : ServiceRegistry
    {
        public WvsCenterRegistry()
        {
            For<WvsCenterOptions>().Use(c =>
            {
                var configBuilder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("WvsCenter.example.json")
                    .AddJsonFile("WvsCenter.json", true);
                var config = configBuilder.Build();
                var options = new WvsCenterOptions();

                config.Bind(options);
                return options;
            }).Singleton();
            For(typeof(ISocketFactory<>)).Use(typeof(CenterClientSocketFactory));
            
            For<WvsCenter>().Use<WvsCenter>().Singleton();
        }
    }
}