using System;
using System.Threading.Tasks;
using Edelstein.WvsGame.Fields.Objects;

namespace Edelstein.WvsGame.Commands.Impl
{
    public class ReactorCommand : Command
    {
        public override string Name => "Reactor";
        public override string Description => "Summons a pretty reactor.";

        protected override Task Execute(CommandContext ctx)
        {
            var templates = ctx.User.Socket.WvsGame.ReactorTemplates;
            var reactor = new FieldReactor(templates.Get(Convert.ToInt32(ctx.Args.Dequeue())))
            {
                X = ctx.User.X,
                Y = ctx.User.Y
            };

            ctx.User.Field.Enter(reactor);
            return Task.CompletedTask;
        }
    }
}