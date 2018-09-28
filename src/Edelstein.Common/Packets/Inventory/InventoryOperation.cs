using System;
using Edelstein.Database.Entities.Inventory;
using Edelstein.Network.Packets;

namespace Edelstein.Common.Packets.Inventory
{
    public abstract class InventoryOperation : IEncodable
    {
        protected readonly InventoryOperationType Type;
        protected readonly ItemInventoryType Inventory;
        protected readonly short Slot;

        public InventoryOperation(InventoryOperationType type, ItemInventoryType inventory, short slot)
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