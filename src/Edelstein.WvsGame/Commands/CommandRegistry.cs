using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CSharpx;
using Edelstein.WvsGame.Fields.Objects.Users;

namespace Edelstein.WvsGame.Commands
{
    public class CommandRegistry : Command<CommandOption>
    {
        public override string Name => "Registry";
        public override string Description => "The registry that holds all commands.";

        public CommandRegistry()
        {
            Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.Namespace.Equals($"{this.GetType().Namespace}.Impl"))
                .Where(t => t.GetInterfaces().Contains(typeof(ICommand)))
                .Where(t => t.GetConstructor(Type.EmptyTypes) != null)
                .Select(Activator.CreateInstance)
                .Cast<ICommand>()
                .ForEach(Commands.Add);
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