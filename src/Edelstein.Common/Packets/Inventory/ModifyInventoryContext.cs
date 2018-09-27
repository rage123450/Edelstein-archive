using System.Collections.Generic;
using System.Linq;
using Edelstein.Common.Packets.Inventory.Operations;
using Edelstein.Database.Entities;
using Edelstein.Database.Entities.Inventory;
using Edelstein.Network.Packets;

namespace Edelstein.Common.Packets.Inventory
{
    public class ModifyInventoryContext : IEncodable
    {
        private readonly Character _character;
        private readonly ICollection<InventoryOperation> _operations;

        public ModifyInventoryContext(Character character)
        {
            _character = character;
            _operations = new List<InventoryOperation>();
        }

        public void Set(ItemInventoryType type, ItemSlot item, short slot)
        {
            var inventory = _character.GetInventory(type);
            var inventoryItems = inventory.Items;
            var existingItem = inventoryItems.SingleOrDefault(i => i.Slot == slot);

            if (existingItem != null) Remove(type, existingItem);

            item.Slot = slot;
            inventoryItems.Add(item);
            _operations.Add(new InventoryAddOperation(getModifyInventoryType(type), item));
        }

        public void Remove(ItemInventoryType type, short slot)
        {
            var inventory = _character.GetInventory(type);
            var inventoryItems = inventory.Items;
            var item = inventoryItems.SingleOrDefault(i => i.Slot == slot);

            if (item != null) Remove(type, item);
        }

        public void Remove(ItemInventoryType type, ItemSlot item)
        {
            var inventory = _character.GetInventory(type);
            var inventoryItems = inventory.Items;

            inventoryItems.Remove(item);
            _operations.Add(new InventoryRemoveOperation(getModifyInventoryType(type), item.Slot));
        }

        public void Move(ItemInventoryType type, short fromSlot, short toSlot)
        {
            var inventory = _character.GetInventory(type);
            var inventoryItems = inventory.Items;
            var item = inventoryItems.SingleOrDefault(i => i.Slot == fromSlot);

            if (item != null) Move(type, item, toSlot);
        }

        public void Move(ItemInventoryType type, ItemSlot item, short toSlot)
        {
            var inventory = _character.GetInventory(type);
            var inventoryItems = inventory.Items;
            var existingItem = inventoryItems.SingleOrDefault(i => i.Slot == toSlot);
            var fromSlot = item.Slot;

            if (existingItem != null)
            {
                // TODO: bundle items

                existingItem.Slot = fromSlot;
            }

            item.Slot = toSlot;
            _operations.Add(new InventoryMoveOperation(getModifyInventoryType(type), fromSlot, toSlot));
        }

        private ModifyInventoryType getModifyInventoryType(ItemInventoryType type)
        {
            switch (type)
            {
                case ItemInventoryType.Equipped:
                case ItemInventoryType.EquippedCash:
                case ItemInventoryType.EquippedDragon:
                case ItemInventoryType.EquippedMechanic:
                    return ModifyInventoryType.Equipped;
                default:
                    return (ModifyInventoryType) type;
            }
        }

        public void Encode(OutPacket packet)
        {
            packet.Encode<byte>((byte) _operations.Count);
            foreach (var operation in _operations) operation.Encode(packet);
        }
    }
}