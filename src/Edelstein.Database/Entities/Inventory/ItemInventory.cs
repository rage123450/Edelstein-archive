using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Edelstein.Database.Entities.Inventory
{
    public class ItemInventory<T>
        where T : ItemSlot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public ICollection<T> Items { get; set; }
        public byte SlotMax { get; set; }
    }
}