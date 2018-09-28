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
                i.Set(ItemInventoryType.Equipped,
                    new ItemSlotEquip
                    {
                        TemplateID = 1302000,
                        Durability = 100
                    }, 11);

                i.Set(ItemInventoryType.Equipped,
                    new ItemSlotEquip
                    {
                        TemplateID = 1302000,
                        Durability = 100
                    }, 111);
            });

            //ctx.User.ModifyInventory(i => { i.Move(ItemInventoryType.Equip, 1, 2); });

            return Task.CompletedTask;
        }
    }
}