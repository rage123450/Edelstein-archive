using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Edelstein.Database.Entities.Shop
{
    public class NPCShopItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int TemplateID { get; set; }
        public int Position { get; set; }
        public int Price { get; set; }
        public int DiscountRate { get; set; }
        public int TokenTemplateID { get; set; }
        public int TokenPrice { get; set; }
        public int ItemPeriod { get; set; }
        public int LevelLimited { get; set; }
        public float UnitPrice { get; set; }
        public int MaxPerSlot { get; set; }
        public int Stock { get; set; }
        public int Quantity { get; set; }
    }
}