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

        private ModifyInventoryType ModifyInventoryType
        {
            get
            {
                switch (Inventory)
                {
                    default:
                        return (ModifyInventoryType) Inventory;
                    case ItemInventoryType.Equipped:
                    case ItemInventoryType.EquippedCash:
                    case ItemInventoryType.EquippedDragon:
                    case ItemInventoryType.EquippedMechanic:
                        return ModifyInventoryType.Equip;
                }
            }
        }

        private short ModifyInventorySlot
        {
            get
            {
                switch (Inventory)
                {
                    default:
                        return Slot;
                    case ItemInventoryType.Equipped:
                        return (short) -Slot;
                    case ItemInventoryType.EquippedCash:
                        return (short) (-Slot - 100);
                    case ItemInventoryType.EquippedDragon:
                        return (short) (-Slot - 1000);
                    case ItemInventoryType.EquippedMechanic:
                        return (short) (-Slot - 1100);
                }
            }
        }

        public InventoryOperation(InventoryOperationType type, ItemInventoryType inventory, short slot)
        {
            Type = type;
            Inventory = inventory;
            Slot = slot;
        }

        public virtual void Encode(OutPacket packet)
        {
            Console.WriteLine(ModifyInventorySlot);
            packet.Encode<byte>((byte) Type);
            packet.Encode<byte>((byte) ModifyInventoryType);
            packet.Encode<short>(ModifyInventorySlot);
        }
    }
}