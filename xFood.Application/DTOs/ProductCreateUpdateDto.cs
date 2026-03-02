using System.ComponentModel.DataAnnotations;

namespace xFood.Application.DTOs;

public class ProductCreateUpdateDto
{
    [Required, StringLength(120)]
    public string Name { get; set; } = null!;
    [StringLength(1000)]
    public string? Description { get; set; }
    [Range(0, 9999999)]
    public decimal Price { get; set; }
    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
    public string? ImageUrl { get; set; }
    [Required]
    public int CategoryId { get; set; }
}
