using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.WvsGame.Logging;
using Lamar;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Edelstein.WvsGame
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
            var registry = new WvsGameRegistry();
            var container = new Container(registry);
            var wvsGame = container.GetInstance<WvsGame>();

            wvsGame.Run().Wait();

            Console.CancelKeyPress += (sender, e) =>
            {
                Logger.Info("Running shutdown sequence..");
                Task.WhenAll(wvsGame.GameServer.Sockets.Select(s => s.Channel.CloseAsync()));
                Task.WhenAll(
                    wvsGame.GameServer.BossGroup.ShutdownGracefullyAsync(),
                    wvsGame.GameServer.WorkerGroup.ShutdownGracefullyAsync(),
                    wvsGame.InteropClient.WorkerGroup.ShutdownGracefullyAsync()
                ).Wait();
                Logger.Info("Application exited");
            };
            wvsGame.GameServer.Channel.CloseCompletion.Wait();
        }
    }
}