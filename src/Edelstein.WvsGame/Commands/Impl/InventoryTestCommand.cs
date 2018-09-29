using System.Threading.Tasks;
using Edelstein.Database.Entities.Inventory;

namespace Edelstein.WvsGame.Commands.Impl
{
    public class InventoryTestCommand : Command
    {
        public override string Name => "InventoryTest";
        public override string Description => "A debugging command for testing inventories.";

        protected override Task Execute(CommandContext ctx)
        {
            ctx.User.ModifyInventory(i =>
            {
                i.Set(ItemInventoryType.Etc,
                    new ItemSlotBundle
                    {
                        TemplateID = 4000000,
                        Number = 3,
                        MaxNumber = 5
                    }, 1);
                i.Set(ItemInventoryType.Etc,
                    new ItemSlotBundle
                    {
                        TemplateID = 4000000,
                        Number = 5,
                        MaxNumber = 5
                    }, 2);
                i.Set(ItemInventoryType.Etc,
                    new ItemSlotBundle
                    {
                        TemplateID = 4000000,
                        Number = 9,
                        MaxNumber = 5
                    }, 3);
            });

            //ctx.User.ModifyInventory(i => { i.Move(ItemInventoryType.Equip, 1, 2); });

            return Task.CompletedTask;
        }
    }
}