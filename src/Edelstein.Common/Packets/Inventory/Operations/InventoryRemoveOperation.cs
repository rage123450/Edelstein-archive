namespace Edelstein.Common.Packets.Inventory.Operations
{
    public class InventoryRemoveOperation : InventoryOperation
    {
        public InventoryRemoveOperation(ModifyInventoryType inventory, short slot)
            : base(InventoryOperationType.Remove, inventory, slot)
        {
        }
    }
}