using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xFood.Domain.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; }

        [Required, StringLength(150)]
        public string Email { get; set; }

        [Required, StringLength(150)]
        public string Password { get; set; }

        [Required]
        public DateTime DateBirth { get; set; }

        // ✅ Relação com TypeUser
        [Display(Name = "User Type")]
        public int TypeUserId { get; set; }   // FK clara
        public virtual TypeUser? TypeUser { get; set; } // Navigation singular

        public bool Active { get; set; } = true;
    }

}
