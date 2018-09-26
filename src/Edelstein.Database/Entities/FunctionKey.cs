using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Edelstein.Database.Entities
{
    public class FunctionKey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int Key { get; set; }
        public byte Type { get; set; }
        public int Action { get; set; }
    }
}