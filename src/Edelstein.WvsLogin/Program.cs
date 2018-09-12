using System.IO;
using Lamar;
using Microsoft.Extensions.Configuration;

namespace Edelstein.WvsLogin
{
    class Program
    {
        static void Main(string[] args)
        {
            var registry = new WvsLoginRegistry();
            var container = new Container(registry);

            container.GetInstance<WvsLogin>().Run().Wait();
        }
    }
}