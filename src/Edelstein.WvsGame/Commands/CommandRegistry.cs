using System.Threading.Tasks;
using Edelstein.WvsGame.Commands.Impl;

namespace Edelstein.WvsGame.Commands
{
    public class CommandRegistry : Command
    {
        public override string Name => "Registry";
        public override string Description => "The registry that holds all commands.";

        public CommandRegistry()
        {
            Commands.Add(new StatCommand());
            Commands.Add(new ItemCommand());
            Commands.Add(new ValidateStatCommand());
            Commands.Add(new BuffCommand());
            Commands.Add(new ShopCommand());
            Commands.Add(new MsgBoxCommand());
            Commands.Add(new MapCommand());
            Commands.Add(new ADBoardCommand());
            Commands.Add(new SuperSpeedCommand());
        }

        protected override Task Execute(CommandContext ctx)
        {
            return Task.CompletedTask;
        }
    }
}