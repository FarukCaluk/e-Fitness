using eFitness.Application.Common.Interfaces;
using eFitness.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.WorkoutPlans.Queries.GetWorkoutPlans;

public record GetWorkoutPlansQuery(int PageNumber, int PageSize, int? TrainerId, int? MemberId, bool? IsActive)
    : IRequest<PaginatedList<WorkoutPlanListItemDto>>;

public class GetWorkoutPlansQueryHandler : IRequestHandler<GetWorkoutPlansQuery, PaginatedList<WorkoutPlanListItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetWorkoutPlansQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<WorkoutPlanListItemDto>> Handle(GetWorkoutPlansQuery request, CancellationToken cancellationToken)
    {
        var query = _context.WorkoutPlans
            .Include(p => p.Trainer).ThenInclude(t => t.User)
            .Include(p => p.Member).ThenInclude(m => m.User)
            .AsQueryable();

        if (request.TrainerId is not null)
        {
            query = query.Where(p => p.TrainerId == request.TrainerId);
        }

        if (request.MemberId is not null)
        {
            query = query.Where(p => p.MemberId == request.MemberId);
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
