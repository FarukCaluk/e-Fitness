using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.WorkoutPlans.Queries.GetWorkoutPlanById;

public record GetWorkoutPlanByIdQuery(int Id) : IRequest<WorkoutPlanDetailDto>;

public class GetWorkoutPlanByIdQueryHandler : IRequestHandler<GetWorkoutPlanByIdQuery, WorkoutPlanDetailDto>
{
    private readonly IApplicationDbContext _context;

    public GetWorkoutPlanByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<WorkoutPlanDetailDto> Handle(GetWorkoutPlanByIdQuery request, CancellationToken cancellationToken)
    {
        var plan = await _context.WorkoutPlans
            .Include(p => p.Trainer).ThenInclude(t => t.User)
            .Include(p => p.Member).ThenInclude(m => m.User)
            .Include(p => p.Exercises).ThenInclude(e => e.Exercise)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (plan is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.WorkoutPlan), request.Id);
        }

        return WorkoutPlanDetailDto.FromEntity(plan);
    }
}
