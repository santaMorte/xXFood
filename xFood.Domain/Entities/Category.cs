using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xFood.Domain.Entities
{
    public class Category
    {
        [Key] public int Id { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; } = null!;

        // Se for obrigatória, deixe não-nullable:
        [Required, StringLength(500)]
        public string Description { get; set; } = null!;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }

}
