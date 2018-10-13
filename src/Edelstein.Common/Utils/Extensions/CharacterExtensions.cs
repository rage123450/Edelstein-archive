using System.Linq;
using Edelstein.Common.Packets.Inventory.Exceptions;
using Edelstein.Database.Entities;
using Edelstein.Database.Entities.Inventory;

namespace Edelstein.Common.Utils.Extensions
{
    public static class CharacterExtensions
    {
        public static bool HasSlotFor(this Character c, ItemSlot item)
        {
            return HasSlotFor(
                c,
                item.ItemInventory?.Type ?? (ItemInventoryType) (item.TemplateID / 1000000),
                item
            );
        }

        public static bool HasSlotFor(this Character c, ItemInventoryType type, ItemSlot item)
        {
            var inventory = c.GetInventory(type);

            var usedSlots = inventory.Items
                .Select<ItemSlot, int>(i => i.Position)
                .Where(s => s > 0)
                .Where(s => s <= inventory.SlotMax)
                .ToList();
            var unusedSlots = Enumerable.Range(1, inventory.SlotMax)
                .Except(usedSlots)
                .ToList();
            return unusedSlots.Count != 0;
        }

        public static bool HasItem(this Character c, int templateID)
        {
            return HasItem(
                c,
                (ItemInventoryType) (templateID / 1000000),
                templateID
            );
        }

        public static bool HasItem(this Character c, ItemInventoryType type, int templateID)
        {
            return GetItemCount(c, type, templateID) > 0;
        }

        public static int GetItemCount(this Character c, int templateID)
        {
            return GetItemCount(
                c,
                (ItemInventoryType) (templateID / 1000000),
                templateID
            );
        }

        public static int GetItemCount(this Character c, ItemInventoryType type, int templateID)
        {
            var inventory = c.GetInventory(type);

            return inventory.Items
                .Select(i =>
                {
                    if (i is ItemSlotBundle bundle)
                        return bundle.Number;
                    return 1;
                })
                .Sum();
        }
    }
}