using System;
using System.Threading.Tasks;

namespace Edelstein.WvsGame.Commands.Impl
{
    public class ShopCommand : Command
    {
        public override string Name => "shop";
        public override string Description => "Opens a npc shop";

        protected override Task Execute(CommandContext ctx)
        {
            var templateID = Convert.ToInt32(ctx.Args.Dequeue());
            var shop = ctx.User.Socket.WvsGame.NPCShops[templateID];

            ctx.User.Dialogue = shop;
            return Task.CompletedTask;
        }
    }
}