using Lamar;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Edelstein.WvsGame
{
    class Program
    {
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

            wvsGame.InteropClient.Channel.CloseCompletion.Wait();
        }
    }
}