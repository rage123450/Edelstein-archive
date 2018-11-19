using System.Threading.Tasks;
using Edelstein.WvsGame.Commands.Impl;
using Edelstein.WvsGame.Fields.Objects.Users;

namespace Edelstein.WvsGame.Commands
{
    public class CommandRegistry : Command<CommandOption>
    {
        public override string Name => "Registry";
        public override string Description => "The registry that holds all commands.";

        public CommandRegistry()
        {
            Commands.Add(new MapCommand());
        }

        public override Task Execute(FieldUser user, CommandOption option)
        {
            return Task.CompletedTask;
        }
    }
}