using eFitness.Application.Common.Interfaces;
using eFitness.Application.Common.Models;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.WorkoutPlans.Queries.GetWorkoutPlans;

public record GetWorkoutPlansQuery(int PageNumber, int PageSize, int? TrainerId, int? MemberId, bool? IsActive)
    : IRequest<PaginatedList<WorkoutPlanListItemDto>>;

public class GetWorkoutPlansQueryHandler : IRequestHandler<GetWorkoutPlansQuery, PaginatedList<WorkoutPlanListItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetWorkoutPlansQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<PaginatedList<WorkoutPlanListItemDto>> Handle(GetWorkoutPlansQuery request, CancellationToken cancellationToken)
    {
        var query = _context.WorkoutPlans
            .Include(p => p.Trainer).ThenInclude(t => t.User)
            .Include(p => p.Member).ThenInclude(m => m.User)
            .AsQueryable();

        var trainerIdFilter = request.TrainerId;
        var memberIdFilter = request.MemberId;

        if (_currentUserService.Role == UserRole.Trainer)
        {
            var trainer = await _context.Trainers.FirstOrDefaultAsync(t => t.UserId == _currentUserService.UserId, cancellationToken);
            trainerIdFilter = trainer?.Id ?? -1;
        }
        else if (_currentUserService.Role == UserRole.Client)
        {
            var member = await _context.Members.FirstOrDefaultAsync(m => m.UserId == _currentUserService.UserId, cancellationToken);
            memberIdFilter = member?.Id ?? -1;
        }

        if (trainerIdFilter is not null)
        {
            query = query.Where(p => p.TrainerId == trainerIdFilter);
        }

        if (memberIdFilter is not null)
        {
            query = query.Where(p => p.MemberId == memberIdFilter);
        }

        if (request.IsActive is not null)
        {
            query = query.Where(p => p.IsActive == request.IsActive);
        }

        query = query.OrderByDescending(p => p.StartDate);

        var paged = await PaginatedList<Domain.Entities.WorkoutPlan>.CreateAsync(query, request.PageNumber, request.PageSize);
        var items = paged.Items.Select(WorkoutPlanListItemDto.FromEntity).ToList();

        return new PaginatedList<WorkoutPlanListItemDto>(items, paged.TotalCount, paged.PageNumber, paged.PageSize);
    }
}
