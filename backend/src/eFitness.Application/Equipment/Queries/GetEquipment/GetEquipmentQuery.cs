using eFitness.Application.Common.Interfaces;
using eFitness.Application.Common.Models;
using eFitness.Domain.Enums;
using MediatR;

namespace eFitness.Application.Equipment.Queries.GetEquipment;

public record GetEquipmentQuery(int PageNumber, int PageSize, string? SearchTerm, EquipmentCategory? Category)
    : IRequest<PaginatedList<EquipmentDto>>;

public class GetEquipmentQueryHandler : IRequestHandler<GetEquipmentQuery, PaginatedList<EquipmentDto>>
{
    private readonly IApplicationDbContext _context;

    public GetEquipmentQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<EquipmentDto>> Handle(GetEquipmentQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Equipment.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim().ToLower();
            query = query.Where(e => e.Name.ToLower().Contains(term));
        }

        if (request.Category is not null)
        {
            query = query.Where(e => e.Category == request.Category);
        }

        query = query.OrderBy(e => e.Name);

        var paged = await PaginatedList<Domain.Entities.Equipment>.CreateAsync(query, request.PageNumber, request.PageSize);
        var items = paged.Items.Select(EquipmentDto.FromEntity).ToList();

        return new PaginatedList<EquipmentDto>(items, paged.TotalCount, paged.PageNumber, paged.PageSize);
    }
}
