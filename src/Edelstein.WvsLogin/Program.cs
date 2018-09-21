using System.Linq;
using System.Threading.Tasks;
using Lamar;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Edelstein.WvsLogin
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .CreateLogger();
            var registry = new WvsLoginRegistry();
            var container = new Container(registry);
            var wvsLogin = container.GetInstance<WvsLogin>();

            wvsLogin.Run().Wait();

            Task.WhenAll(
                wvsLogin.InteropClients.Select(c => c.Channel.CloseCompletion)
            ).Wait();
            
            wvsLogin.GameServer.Channel.CloseAsync();
            wvsLogin.GameServer.Channel.CloseCompletion.Wait();
        }
    }
}