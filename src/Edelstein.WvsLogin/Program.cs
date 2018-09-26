using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.WvsLogin.Logging;
using Lamar;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Edelstein.WvsLogin
{
    class Program
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        
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

            Console.CancelKeyPress += (sender, e) =>
            {
                Logger.Info("Running shutdown sequence..");
                Task.WhenAll(wvsLogin.GameServer.Sockets.Select(s => s.Channel.CloseAsync()));
                Task.WhenAll(
                    wvsLogin.GameServer.BossGroup.ShutdownGracefullyAsync(),
                    wvsLogin.GameServer.WorkerGroup.ShutdownGracefullyAsync()
                ).Wait();
                Task.WhenAll(wvsLogin.InteropClients.Select(c => c.WorkerGroup.ShutdownGracefullyAsync()));
                Logger.Info("Application exited");
            };
            wvsLogin.GameServer.Channel.CloseCompletion.Wait();
        }
    }
}