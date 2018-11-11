using System;
using System.Threading.Tasks;
using Edelstein.WvsGame.Fields.Objects.Users.Effects;

namespace Edelstein.WvsGame.Commands.Impl
{
    public class EffectCommand : Command
    {
        public override string Name => "Effect";
        public override string Description => "Show your bling blings.";

        protected override Task Execute(CommandContext ctx)
        {
            return ctx.User.Effect((UserEffectType) Convert.ToInt32(ctx.Args.Dequeue()));
        }
    }
}