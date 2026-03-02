using Microsoft.EntityFrameworkCore;
using xFood.Application.DTOs;
using xFood.Application.Interfaces;
using xFood.Domain.Entities;
using xFood.Infrastructure.Persistence;

namespace xFood.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly xFoodDbContext _ctx;
    public CategoryRepository(xFoodDbContext ctx) => _ctx = ctx;

    public async Task<List<CategoryDto>> GetAllAsync()
        => await _ctx.Categories.AsNoTracking()
           .OrderBy(c => c.Name)
           .Select(c => new CategoryDto(c.Id, c.Name, c.Description))
           .ToListAsync();

    public async Task<CategoryDto?> GetByIdAsync(int id)
        => await _ctx.Categories.AsNoTracking()
           .Where(c => c.Id == id)
           .Select(c => new CategoryDto(c.Id, c.Name, c.Description))
           .FirstOrDefaultAsync();

    public Task<bool> AnyProductsAsync(int categoryId)
        => _ctx.Products.AnyAsync(p => p.CategoryId == categoryId);

    public async Task<int> CreateAsync(CategoryDto dto)
    {
        var e = new Category { Name = dto.Name, Description = dto.Description };
        _ctx.Categories.Add(e);
        await _ctx.SaveChangesAsync();
        return e.Id;
    }

    public async Task UpdateAsync(CategoryDto dto)
    {
        var e = await _ctx.Categories.FindAsync(dto.Id);
        if (e is null) throw new KeyNotFoundException("Categoria não encontrada.");
        e.Name = dto.Name;
        e.Description = dto.Description;
        await _ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var e = await _ctx.Categories.FindAsync(id);
        if (e is null) return;
        _ctx.Categories.Remove(e);
        await _ctx.SaveChangesAsync();
    }
}
