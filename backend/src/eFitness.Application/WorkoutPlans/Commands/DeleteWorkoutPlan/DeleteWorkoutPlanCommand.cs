using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.WorkoutPlans.Commands.DeleteWorkoutPlan;

public record DeleteWorkoutPlanCommand(int Id) : IRequest;

public class DeleteWorkoutPlanCommandHandler : IRequestHandler<DeleteWorkoutPlanCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteWorkoutPlanCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteWorkoutPlanCommand request, CancellationToken cancellationToken)
    {
        var plan = await _context.WorkoutPlans.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (plan is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.WorkoutPlan), request.Id);
        }

        _context.WorkoutPlans.Remove(plan);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
