using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Edelstein.WvsLogin
{
    class Program
    {
        static void Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("WvsLogin.example.json")
                .AddJsonFile("WvsLogin.json", true);
            var config = configBuilder.Build();
            var options = new WvsLoginOptions();

            config.Bind(options);

            var wvsLogin = new WvsLogin(options);

            wvsLogin.Run().Wait();
        }
    }
}