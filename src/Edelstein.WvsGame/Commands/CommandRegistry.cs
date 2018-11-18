using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.WvsGame.Commands.Impl;
using Edelstein.WvsGame.Commands.Impl.Util;
using MoreLinq;
using MoreLinq.Extensions;

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
            Commands.Add(new EffectCommand());
            Commands.Add(new ReactorCommand());

            MoreEnumerable.TraverseBreadthFirst((Command) this, c => c.Commands.AsEnumerable())
                .ToList()
                .ForEach(c =>
                {
                    c.Commands.Add(new HelpCommand(c));
                    c.Commands.Add(new AliasCommand(c));
                });
        }

        protected override Task Execute(CommandContext ctx)
        {
            return Task.CompletedTask;
        }
    }
}