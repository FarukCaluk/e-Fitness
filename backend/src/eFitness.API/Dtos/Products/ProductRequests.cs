using eFitness.Domain.Enums;

namespace eFitness.API.Dtos.Products;

public record CreateProductRequest(string Name, string? Description, decimal Price, ProductCategory Category, int StockQuantity, string? ImageUrl);

public record UpdateProductRequest(string Name, string? Description, decimal Price, ProductCategory Category, int StockQuantity, string? ImageUrl, bool IsActive);
