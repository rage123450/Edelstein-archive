using System;
using System.Threading.Tasks;
using Edelstein.Common.Utils.Items;
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

            var quantity = (short) (ctx.Args.Count > 0 ? Convert.ToInt16(ctx.Args.Dequeue()) : 1);
            var variation = (ItemVariationType) (ctx.Args.Count > 0 ? Convert.ToInt16(ctx.Args.Dequeue()) : 0);
            
            ctx.User.ModifyInventory(i =>
                i.Add(
                    templates.Get(templateID),
                    quantity,
                    variation
                )
            );

            return Task.CompletedTask;
        }
    }
}