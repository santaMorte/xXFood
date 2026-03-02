namespace xFood.Application.DTOs;

public record ProductDto(
    int Id,
    string Name,
    string? Description,
    decimal Price,
    int Stock,
    string? ImageUrl,
    int CategoryId,
    string? CategoryName
);
