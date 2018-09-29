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

            ctx.User.ModifyInventory(i =>
            {
                i.Add(new ItemSlotBundle
                {
                    TemplateID = templateID,
                    Number = 3,
                    MaxNumber = 50
                });
            });

            return Task.CompletedTask;
        }
    }
}