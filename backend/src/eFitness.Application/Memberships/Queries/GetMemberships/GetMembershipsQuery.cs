using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using eFitness.Application.Common.Models;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Memberships.Queries.GetMemberships;

public record GetMembershipsQuery(int PageNumber, int PageSize, int? MemberId, MembershipStatus? Status)
    : IRequest<PaginatedList<MembershipDto>>;

public class GetMembershipsQueryHandler : IRequestHandler<GetMembershipsQuery, PaginatedList<MembershipDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetMembershipsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<PaginatedList<MembershipDto>> Handle(GetMembershipsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Memberships
            .Include(m => m.Member).ThenInclude(mem => mem.User)
            .Include(m => m.MembershipPlan)
            .AsQueryable();

        var memberIdFilter = request.MemberId;

        if (_currentUserService.Role == UserRole.Client)
        {
            if (_currentUserService.UserId is null)
            {
                throw new ForbiddenAccessException();
            }

            var member = await _context.Members.FirstOrDefaultAsync(m => m.UserId == _currentUserService.UserId, cancellationToken);
            memberIdFilter = member?.Id ?? -1;
        }

        if (memberIdFilter is not null)
        {
            query = query.Where(m => m.MemberId == memberIdFilter);
        }

        if (request.Status is not null)
        {
            query = query.Where(m => m.Status == request.Status);
        }

        query = query.OrderByDescending(m => m.StartDate);

        var paged = await PaginatedList<Domain.Entities.Membership>.CreateAsync(query, request.PageNumber, request.PageSize);
        var items = paged.Items.Select(MembershipDto.FromEntity).ToList();

        return new PaginatedList<MembershipDto>(items, paged.TotalCount, paged.PageNumber, paged.PageSize);
    }
}
