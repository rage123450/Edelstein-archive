using Edelstein.Database.Entities.Inventory;
using Edelstein.Network.Packets;

namespace Edelstein.Common.Packets.Inventory.Operations
{
    public class InventoryUpdateQuantityOperation : InventoryOperation
    {
        private readonly short _quantity;

        public InventoryUpdateQuantityOperation(ItemInventoryType inventory, short slot, short quantity)
            : base(InventoryOperationType.UpdateQuantity, inventory, slot)
        {
            _quantity = quantity;
        }

        public override void Encode(OutPacket packet)
        {
            base.Encode(packet);
            
            packet.Encode<short>(_quantity);
        }
    }
}