using System;
using System.Threading.Tasks;
using Edelstein.Database.Entities.Inventory;

namespace Edelstein.WvsGame.Commands.Impl
{
    public class ItemCommand : Command
    {
        public override string Name => "Item";
        public override string Description => "Creates an item into your inventory.";

        protected override Task Execute(CommandContext ctx)
        {
            var templateID = Convert.ToInt32(ctx.Args.Dequeue());
            var templates = ctx.User.Socket.WvsGame.ItemTemplates;

            ctx.User.ModifyInventory(i =>
                i.Add(
                    templates.Get(templateID),
                    (short) (ctx.Args.Count > 0 ? Convert.ToInt16(ctx.Args.Dequeue()) : 1)
                )
            );

            return Task.CompletedTask;
        }
    }
}