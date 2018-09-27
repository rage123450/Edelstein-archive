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
                i.Set(new ItemSlotEquip
                {
                    TemplateID = 1302000,
                    Durability = 100
                }, ItemInventoryType.Equip, 1);

               i.Remove(ItemInventoryType.Equip, 1);
            });

            return Task.CompletedTask;
        }
    }
}