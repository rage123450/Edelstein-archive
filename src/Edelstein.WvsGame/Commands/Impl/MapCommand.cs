using System;
using System.Threading.Tasks;

namespace Edelstein.WvsGame.Commands.Impl
{
    public class MapCommand : Command
    {
        public override string Name => "Map";
        public override string Description => "Warp-a-doodledoo!";
        
        protected override Task Execute(CommandContext ctx)
        {
            var factory = ctx.User.Socket.WvsGame.FieldFactory;
            var field = factory.Get(Convert.ToInt32(ctx.Args.Dequeue()));
            
            field.Enter(ctx.User);
            return Task.CompletedTask;
        }
    }
}