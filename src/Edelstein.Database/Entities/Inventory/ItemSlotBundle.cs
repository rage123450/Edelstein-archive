using System.ComponentModel.DataAnnotations;

namespace Edelstein.Database.Entities.Inventory
{
    public class ItemSlotBundle : ItemSlot
    {
        public short Number { get; set; }
        public short Attribute { get; set; }

        [MaxLength(13)] public string Title { get; set; }
    }
}