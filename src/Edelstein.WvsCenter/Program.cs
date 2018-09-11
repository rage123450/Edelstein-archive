using System.IO;
using Microsoft.Extensions.Configuration;

namespace Edelstein.WvsCenter
{
    class Program
    {
        static void Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("WvsCenter.example.json")
                .AddJsonFile("WvsCenter.json", true);
            var config = configBuilder.Build();
            var options = new WvsCenterOptions();

            config.Bind(options);

            var wvsCenter = new WvsCenter(options);

            wvsCenter.Run().Wait();
        }
    }
}