using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Edelstein.Database.Entities
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(13)] public string Username { get; set; }
        [MaxLength(128)] public string Password { get; set; }

        public ICollection<Character> Characters { get; set; }
    }
}