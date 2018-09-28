using Edelstein.Database.Entities.Inventory;

namespace Edelstein.Common.Packets.Inventory.Operations
{
    public class InventoryRemoveOperation : InventoryOperation
    {
        public InventoryRemoveOperation(ItemInventoryType inventory, short slot)
            : base(InventoryOperationType.Remove, inventory, slot)
        {
        }
    }
}