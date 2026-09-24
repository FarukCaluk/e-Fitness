using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using eFitness.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Members.Queries.GetMyClients;

public record GetMyClientsQuery(int PageNumber, int PageSize, string? SearchTerm) : IRequest<PaginatedList<MemberListItemDto>>;

public class GetMyClientsQueryHandler : IRequestHandler<GetMyClientsQuery, PaginatedList<MemberListItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetMyClientsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<PaginatedList<MemberListItemDto>> Handle(GetMyClientsQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is null)
        {
            throw new ForbiddenAccessException();
        }

        var trainer = await _context.Trainers.FirstOrDefaultAsync(t => t.UserId == _currentUserService.UserId, cancellationToken);

        if (trainer is null)
        {
            throw new ForbiddenAccessException();
        }

        var query = _context.Members
            .Include(m => m.User)
            .Include(m => m.AssignedTrainer).ThenInclude(t => t!.User)
            .Include(m => m.Memberships).ThenInclude(ms => ms.MembershipPlan)
            .Where(m => m.AssignedTrainerId == trainer.Id)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim().ToLower();
            query = query.Where(m => m.User.FirstName.ToLower().Contains(term) || m.User.LastName.ToLower().Contains(term));
        }

        query = query.OrderBy(m => m.User.FirstName);

        var paged = await PaginatedList<Domain.Entities.Member>.CreateAsync(query, request.PageNumber, request.PageSize);
        var items = paged.Items.Select(MemberListItemDto.FromEntity).ToList();

        return new PaginatedList<MemberListItemDto>(items, paged.TotalCount, paged.PageNumber, paged.PageSize);
    }
}
