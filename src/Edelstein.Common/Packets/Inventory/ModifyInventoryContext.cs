using System;
using System.Collections.Generic;
using System.Linq;
using Edelstein.Common.Packets.Inventory.Exceptions;
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

        public void Add(ItemInventoryType type, ItemSlot item)
        {
            var inventory = _character.GetInventory(type);

            switch (item)
            {
                case ItemSlotBundle bundle:
                    if (bundle.Number < 1) bundle.Number = 1;
                    if (bundle.MaxNumber < 1) bundle.MaxNumber = 1;

                    var mergeableSlots = inventory.Items
                        .OfType<ItemSlotBundle>()
                        .Where(b => b.TemplateID == bundle.TemplateID)
                        .Where(b => b.Attribute == bundle.Attribute)
                        .Where(b => b.Title == bundle.Title)
                        .Where(b => b.Number != b.MaxNumber)
                        .Where(b => b.Slot > 0)
                        .Where(b => b.Slot <= inventory.SlotMax)
                        .ToList();

                    if (mergeableSlots.Count > 0)
                    {
                        var existingBundle = mergeableSlots.First();

                        var count = bundle.Number + existingBundle.Number;
                        var maxNumber = existingBundle.MaxNumber;

                        if (count > maxNumber)
                        {
                            var leftover = count - maxNumber;

                            bundle.Number = (short) leftover;
                            existingBundle.Number = maxNumber;
                            UpdateQuantity(existingBundle);
                            Add(bundle);
                            return;
                        }

                        existingBundle.Number += bundle.Number;
                        UpdateQuantity(existingBundle);
                        return;
                    }

                    goto default;
                default:
                    var usedSlots = inventory.Items
                        .Select<ItemSlot, int>(i => i.Slot)
                        .Where(s => s > 0)
                        .Where(s => s <= inventory.SlotMax)
                        .ToList();
                    var unusedSlots = Enumerable.Range(1, inventory.SlotMax - 1)
                        .Except(usedSlots)
                        .ToList();

                    if (unusedSlots.Count == 0) throw new InventoryFullException();

                    Set(type, item, (short) unusedSlots.First());
                    break;
            }
        }

        public void Add(ItemSlot item)
        {
            Add(item.ItemInventory?.Type ?? (ItemInventoryType) (item.TemplateID / 1000000), item);
        }

        public void Set(ItemInventoryType type, ItemSlot item, short slot)
        {
            item.ItemInventory = _character.GetInventory(type);
            item.Slot = slot;
            Set(item);
        }

        public void Set(ItemSlot item)
        {
            var inventory = item.ItemInventory;
            var inventoryItems = inventory.Items;
            var existingItem = inventoryItems.SingleOrDefault(i => i.Slot == item.Slot);

            if (existingItem != null) Remove(existingItem);

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

            if (item is ItemSlotBundle bundle)
            {
                if (existingItem is ItemSlotBundle existingBundle)
                {
                    if (bundle.TemplateID == existingBundle.TemplateID &&
                        bundle.Attribute == existingBundle.Attribute &&
                        bundle.Title == existingBundle.Title)
                    {
                        var count = bundle.Number + existingBundle.Number;
                        var maxNumber = existingBundle.MaxNumber;

                        if (count > maxNumber)
                        {
                            var leftover = count - maxNumber;

                            bundle.Number = (short) leftover;
                            existingBundle.Number = maxNumber;
                            UpdateQuantity(bundle);
                        }
                        else
                        {
                            existingBundle.Number = (short) count;
                            Remove(bundle);
                        }

                        UpdateQuantity(existingBundle);
                        return;
                    }
                }
            }

            if (existingItem != null)
            {
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

        public void UpdateQuantity(ItemSlotBundle bundle)
        {
            _operations.Add(new InventoryUpdateQuantityOperation(
                bundle.ItemInventory.Type,
                bundle.Slot,
                bundle.Number)
            );
        }

        public void Encode(OutPacket packet)
        {
            packet.Encode<byte>((byte) _operations.Count);
            foreach (var operation in _operations) operation.Encode(packet);
        }
    }
}