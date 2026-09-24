using eFitness.Application.Common.Interfaces;
using eFitness.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.MembershipPlans.Queries.GetMembershipPlans;

public record GetMembershipPlansQuery(int PageNumber, int PageSize, bool? IsActive)
    : IRequest<PaginatedList<MembershipPlanDto>>;

public class GetMembershipPlansQueryHandler : IRequestHandler<GetMembershipPlansQuery, PaginatedList<MembershipPlanDto>>
{
    private readonly IApplicationDbContext _context;

    public GetMembershipPlansQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<MembershipPlanDto>> Handle(GetMembershipPlansQuery request, CancellationToken cancellationToken)
    {
        var query = _context.MembershipPlans.Include(p => p.Features).AsQueryable();

        if (request.IsActive is not null)
        {
            query = query.Where(p => p.IsActive == request.IsActive);
        }

        query = query.OrderBy(p => p.Price);

        var paged = await PaginatedList<Domain.Entities.MembershipPlan>.CreateAsync(query, request.PageNumber, request.PageSize);
        var items = paged.Items.Select(MembershipPlanDto.FromEntity).ToList();

        return new PaginatedList<MembershipPlanDto>(items, paged.TotalCount, paged.PageNumber, paged.PageSize);
    }
}
