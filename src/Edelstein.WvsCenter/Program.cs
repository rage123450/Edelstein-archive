using Lamar;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Edelstein.WvsCenter
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .CreateLogger();
            var registry = new WvsCenterRegistry();
            var container = new Container(registry);

            container.GetInstance<WvsCenter>().Run().Wait();
        }
    }
}