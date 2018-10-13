using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Edelstein.Database.Entities.Shop
{
    public class NPCShop
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        
        public int TemplateID { get; set; }
        public ICollection<NPCShopItem> Items { get; set; } = new List<NPCShopItem>();
    }
}