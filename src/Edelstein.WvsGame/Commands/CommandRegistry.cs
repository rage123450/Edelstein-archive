using System.Collections.Generic;
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
            Commands.Add(new StatCommand());
        }

        public override Task Parse(FieldUser user, IEnumerable<string> args)
        {
            return base.Parse(user, new[] {"--help"});
        }

        public override Task Execute(FieldUser user, CommandOption option)
        {
            return Task.CompletedTask;
        }
    }
}