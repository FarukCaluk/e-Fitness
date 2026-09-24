using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    int Id,
    string Name,
    string? Description,
    decimal Price,
    ProductCategory Category,
    int StockQuantity,
    string? ImageUrl,
    bool IsActive) : IRequest;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Product), request.Id);
        }

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Category = request.Category;
        product.StockQuantity = request.StockQuantity;
        product.ImageUrl = request.ImageUrl;
        product.IsActive = request.IsActive;
        product.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
