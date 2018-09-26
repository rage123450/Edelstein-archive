using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Edelstein.Database.Entities.Inventory
{
    public class ItemInventory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        
        public ItemInventoryType Type { get; set; }
        public byte SlotMax { get; set; }
        
        public ICollection<ItemSlot> Items { get; set; } = new List<ItemSlot>();

        public ItemInventory(ItemInventoryType type, byte slotMax)
        {
            Type = type;
            SlotMax = slotMax;
        }
    }
}