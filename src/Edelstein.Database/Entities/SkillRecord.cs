using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Edelstein.Database.Entities.Types;

namespace Edelstein.Database.Entities
{
    public class SkillRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public Skill Skill { get; set; }
        public int Info { get; set; }
        public int MasterLevel { get; set; }
        public DateTime? DateExpire { get; set; }
    }
}