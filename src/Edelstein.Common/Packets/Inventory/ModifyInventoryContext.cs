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

            if (existingItem != null) Remove(existingItem);

            item.ItemInventory = inventory;
            item.Slot = slot;
            inventoryItems.Add(item);
            _operations.Add(new InventoryAddOperation(
                inventory.Type,
                item)
            );
        }

        public void Remove(ItemInventoryType type, short slot)
        {
            var inventory = _character.GetInventory(type);
            var inventoryItems = inventory.Items;
            var item = inventoryItems.SingleOrDefault(i => i.Slot == slot);

            if (item != null) Remove(item);
        }

        public void Remove(ItemSlot item)
        {
            var inventory = item.ItemInventory;
            var inventoryItems = inventory.Items;

            inventoryItems.Remove(item);
            _operations.Add(new InventoryRemoveOperation(
                inventory.Type,
                item.Slot)
            );
        }

        public void Move(ItemInventoryType type, short fromSlot, short toSlot)
        {
            var inventory = _character.GetInventory(type);
            var inventoryItems = inventory.Items;
            var item = inventoryItems.SingleOrDefault(i => i.Slot == fromSlot);

            if (item != null) Move(item, toSlot);
        }

        public void Move(ItemSlot item, short toSlot)
        {
            var inventory = item.ItemInventory;
            var existingItem = inventory.Items.SingleOrDefault(i => i.Slot == toSlot);
            var fromSlot = item.Slot;

            if (existingItem != null)
            {
                // TODO: bundle items
                existingItem.ItemInventory = inventory;
                existingItem.Slot = fromSlot;
            }

            item.Slot = toSlot;

            _operations.Add(new InventoryMoveOperation(
                inventory.Type,
                fromSlot,
                toSlot)
            );
        }

        public void Encode(OutPacket packet)
        {
            packet.Encode<byte>((byte) _operations.Count);
            foreach (var operation in _operations) operation.Encode(packet);
        }
    }
}