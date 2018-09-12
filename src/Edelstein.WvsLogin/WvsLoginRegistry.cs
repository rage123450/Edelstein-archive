using System.IO;
using Edelstein.Network;
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
                    .AddJsonFile("WvsCenter.example.json")
                    .AddJsonFile("WvsCenter.json", true);
                var config = configBuilder.Build();
                var options = new WvsLoginOptions();

                config.Bind(options);
                return options;
            }).Singleton();
            
            For(typeof(ISocketFactory<>)).Use(typeof(LoginClientSocketFactory)).Named("Interop");
            For(typeof(ISocketFactory<>)).Use(typeof(LoginClientSocketFactory)).Named("Game");
            
            For<WvsLogin>().Use<WvsLogin>().Singleton();
        }
    }
}