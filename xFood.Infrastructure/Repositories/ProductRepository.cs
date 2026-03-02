using Microsoft.EntityFrameworkCore;
using xFood.Application.DTOs;
using xFood.Application.Interfaces;
using xFood.Domain.Entities;
using xFood.Infrastructure.Persistence;

namespace xFood.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly xFoodDbContext _ctx;
    public ProductRepository(xFoodDbContext ctx) => _ctx = ctx;

    public async Task<(int total, List<ProductDto> items)> GetAllAsync(int? categoryId, string? q, int page, int size)
    {
        if (page < 1) page = 1;
        if (size < 1 || size > 100) size = 12;

        var query = _ctx.Products.AsNoTracking().Include(p => p.Category).AsQueryable();

        if (categoryId.HasValue) query = query.Where(p => p.CategoryId == categoryId.Value);
        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            query = query.Where(p => p.Name.Contains(term) || (p.Description != null && p.Description.Contains(term)));
        }

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(p => p.Name)
            .Skip((page - 1) * size)
            .Take(size)
            .Select(p => new ProductDto(p.Id, p.Name, p.Description, p.Price, p.Stock, p.ImageUrl, p.CategoryId, p.Category!.Name))
            .ToListAsync();

        return (total, items);
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
        => await _ctx.Products.AsNoTracking()
           .Include(p => p.Category)
           .Where(p => p.Id == id)
           .Select(p => new ProductDto(p.Id, p.Name, p.Description, p.Price, p.Stock, p.ImageUrl, p.CategoryId, p.Category!.Name))
           .FirstOrDefaultAsync();

    public async Task<int> CreateAsync(ProductCreateUpdateDto dto)
    {
        var e = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock,
            ImageUrl = dto.ImageUrl,
            CategoryId = dto.CategoryId
        };
        _ctx.Products.Add(e);
        await _ctx.SaveChangesAsync();
        return e.Id;
    }

    public async Task UpdateAsync(int id, ProductCreateUpdateDto dto)
    {
        var e = await _ctx.Products.FindAsync(id);
        if (e is null) throw new KeyNotFoundException("Produto não encontrado.");

        e.Name = dto.Name;
        e.Description = dto.Description;
        e.Price = dto.Price;
        e.Stock = dto.Stock;
        e.ImageUrl = dto.ImageUrl;
        e.CategoryId = dto.CategoryId;

        await _ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var e = await _ctx.Products.FindAsync(id);
        if (e is null) return;
        _ctx.Products.Remove(e);
        await _ctx.SaveChangesAsync();
    }
}
