using eFitness.Application.Common.Interfaces;
using eFitness.Domain.Entities;
using eFitness.Domain.Enums;
using MediatR;

namespace eFitness.Application.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string? Description,
    decimal Price,
    ProductCategory Category,
    int StockQuantity,
    string? ImageUrl) : IRequest<int>;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Category = request.Category,
            StockQuantity = request.StockQuantity,
            ImageUrl = request.ImageUrl,
            IsActive = true
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}
