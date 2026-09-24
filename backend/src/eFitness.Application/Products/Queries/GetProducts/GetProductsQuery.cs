using eFitness.Application.Common.Interfaces;
using eFitness.Application.Common.Models;
using eFitness.Domain.Enums;
using MediatR;

namespace eFitness.Application.Products.Queries.GetProducts;

public record GetProductsQuery(
    int PageNumber,
    int PageSize,
    string? SearchTerm,
    ProductCategory? Category,
    bool? IsActive) : IRequest<PaginatedList<ProductDto>>;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PaginatedList<ProductDto>>
{
    private readonly IApplicationDbContext _context;

    public GetProductsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(term));
        }

        if (request.Category is not null)
        {
            query = query.Where(p => p.Category == request.Category);
        }

        if (request.IsActive is not null)
        {
            query = query.Where(p => p.IsActive == request.IsActive);
        }

        query = query.OrderBy(p => p.Name);

        var paged = await PaginatedList<Domain.Entities.Product>.CreateAsync(query, request.PageNumber, request.PageSize);
        var items = paged.Items.Select(ProductDto.FromEntity).ToList();

        return new PaginatedList<ProductDto>(items, paged.TotalCount, paged.PageNumber, paged.PageSize);
    }
}
