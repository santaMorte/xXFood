using xFood.Application.DTOs;

namespace xFood.Application.Interfaces;

public interface IProductRepository
{
    Task<(int total, List<ProductDto> items)> GetAllAsync(int? categoryId, string? q, int page, int size);
    Task<ProductDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(ProductCreateUpdateDto dto);
    Task UpdateAsync(int id, ProductCreateUpdateDto dto);
    Task DeleteAsync(int id);
}
