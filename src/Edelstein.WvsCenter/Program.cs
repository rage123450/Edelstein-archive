using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.WvsCenter.Logging;
using Lamar;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Edelstein.WvsCenter
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
            var registry = new WvsCenterRegistry();
            var container = new Container(registry);
            var wvsCenter = container.GetInstance<WvsCenter>();

            wvsCenter.Run().Wait();
            Console.CancelKeyPress += (sender, e) =>
            {
                Logger.Info("Running shutdown sequence..");
                Task.WhenAll(wvsCenter.InteropServer.Sockets.Select(s => s.Channel.CloseAsync()));
                wvsCenter.InteropServer.WorkerGroup.ShutdownGracefullyAsync().Wait();
                Logger.Info("Application exited");
            };
            wvsCenter.InteropServer.Channel.CloseCompletion.Wait();
        }
    }
}