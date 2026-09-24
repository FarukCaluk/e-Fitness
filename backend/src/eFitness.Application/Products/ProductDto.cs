using eFitness.Domain.Entities;
using eFitness.Domain.Enums;

namespace eFitness.Application.Products;

public record ProductDto(
    int Id,
    string Name,
    string? Description,
    decimal Price,
    ProductCategory Category,
    int StockQuantity,
    string? ImageUrl,
    bool IsActive)
{
    public static ProductDto FromEntity(Product product) => new(
        product.Id,
        product.Name,
        product.Description,
        product.Price,
        product.Category,
        product.StockQuantity,
        product.ImageUrl,
        product.IsActive);
}
