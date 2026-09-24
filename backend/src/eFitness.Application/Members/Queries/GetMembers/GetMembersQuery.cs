using eFitness.Application.Common.Interfaces;
using eFitness.Application.Common.Models;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Members.Queries.GetMembers;

public record GetMembersQuery(
    int PageNumber,
    int PageSize,
    string? SearchTerm,
    MembershipStatus? MembershipStatus) : IRequest<PaginatedList<MemberListItemDto>>;

public class GetMembersQueryHandler : IRequestHandler<GetMembersQuery, PaginatedList<MemberListItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetMembersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<MemberListItemDto>> Handle(GetMembersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Members
            .Include(m => m.User)
            .Include(m => m.AssignedTrainer).ThenInclude(t => t!.User)
            .Include(m => m.Memberships).ThenInclude(ms => ms.MembershipPlan)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim().ToLower();
            query = query.Where(m =>
                m.User.FirstName.ToLower().Contains(term) ||
                m.User.LastName.ToLower().Contains(term) ||
                m.User.Email.ToLower().Contains(term));
        }

        if (request.MembershipStatus is not null)
        {
            query = query.Where(m => m.Memberships.Any(ms => ms.Status == request.MembershipStatus));
        }

        query = query.OrderByDescending(m => m.JoinDate);

        var paged = await PaginatedList<Domain.Entities.Member>.CreateAsync(query, request.PageNumber, request.PageSize);

        var items = paged.Items.Select(MemberListItemDto.FromEntity).ToList();

        return new PaginatedList<MemberListItemDto>(items, paged.TotalCount, paged.PageNumber, paged.PageSize);
    }
}
