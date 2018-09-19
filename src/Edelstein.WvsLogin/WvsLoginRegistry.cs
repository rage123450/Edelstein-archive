using System.IO;
using Edelstein.Network;
using Edelstein.WvsLogin.Sockets;
using Lamar;
using Microsoft.Extensions.Configuration;

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

            For<CenterServerSocketFactory>().Use<CenterServerSocketFactory>();
            For<LoginClientSocketFactory>().Use<LoginClientSocketFactory>();
            
            For<WvsLogin>().Use<WvsLogin>().Singleton();
        }
    }
}