using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xFood.Domain.Entities
{
    public class Product
    {
        [Key] public int Id { get; set; }

        [Required, StringLength(120)]
        public string Name { get; set; } = null!;

        // Se puder ser opcional, remova [Required] e mantenha nullable:
        [StringLength(1000)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 999999999999.99)]
        public decimal Price { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [StringLength(2048)]
        public string? ImageUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
    }

}
