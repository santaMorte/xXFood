using xFood.Application.DTOs;

namespace xFood.Application.Interfaces;

public interface ICategoryRepository
{
    Task<List<CategoryDto>> GetAllAsync();
    Task<CategoryDto?> GetByIdAsync(int id);
    Task<bool> AnyProductsAsync(int categoryId);
    Task<int> CreateAsync(CategoryDto dto);
    Task UpdateAsync(CategoryDto dto);
    Task DeleteAsync(int id);
}
