using CommandLine;

namespace Edelstein.WvsGame.Commands
{
    public class CommandOption
    {
        [Option('v', "verbose", HelpText = "Enables verbose printing.")]
        public bool Verbose { get; set; }
    }
}