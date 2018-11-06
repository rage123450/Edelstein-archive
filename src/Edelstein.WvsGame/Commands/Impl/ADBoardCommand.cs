using System;
using System.Threading.Tasks;

namespace Edelstein.WvsGame.Commands.Impl
{
    public class ADBoardCommand : Command
    {
        public override string Name => "ADBoard";
        public override string Description => "Shows a pretty ad board.";

        protected override Task Execute(CommandContext ctx)
        {
            try
            {
                ctx.User.ADBoard = ctx.Args.Dequeue();
            }
            catch (InvalidOperationException)
            {
                ctx.User.ADBoard = null;
            }

            return Task.CompletedTask;
        }
    }
}