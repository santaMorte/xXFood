using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xFood.Domain.Entities
{
    public class TypeUser
    {
        [Key]
        public int Id { get; set; } // ✅ simplifica, o EF reconhece automaticamente

        [Required, StringLength(150)]
        public string Description { get; set; }

        // ✅ Navigation inversa
        public ICollection<User> Users { get; set; } = new List<User>();
    }

}
