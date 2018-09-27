using System;
using Edelstein.Network.Packets;

namespace Edelstein.Common.Packets.Inventory
{
    public abstract class InventoryOperation : IEncodable
    {
        protected readonly InventoryOperationType Type;
        protected readonly ModifyInventoryType Inventory;
        protected readonly short Slot;

        public InventoryOperation(InventoryOperationType type, ModifyInventoryType inventory, short slot)
        {
            Type = type;
            Inventory = inventory;
            Slot = slot;
        }

        public virtual void Encode(OutPacket packet)
        {
            packet.Encode<byte>((byte) Type);
            packet.Encode<byte>((byte) Inventory);
            packet.Encode<short>(Slot);
        }
    }
}