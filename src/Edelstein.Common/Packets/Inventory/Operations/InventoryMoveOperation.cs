using Edelstein.Database.Entities.Inventory;
using Edelstein.Network.Packets;

namespace Edelstein.Common.Packets.Inventory.Operations
{
    public class InventoryMoveOperation : InventoryOperation
    {
        private readonly short _newSlot;

        public InventoryMoveOperation(ItemInventoryType inventory, short slot, short newSlot)
            : base(InventoryOperationType.Move, inventory, slot)
        {
            _newSlot = newSlot;
        }

        public override void Encode(OutPacket packet)
        {
            base.Encode(packet);

            packet.Encode<short>(_newSlot);
        }
    }
}