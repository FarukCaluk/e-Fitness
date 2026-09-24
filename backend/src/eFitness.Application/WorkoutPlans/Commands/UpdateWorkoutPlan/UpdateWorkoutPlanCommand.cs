using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using eFitness.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.WorkoutPlans.Commands.UpdateWorkoutPlan;

public record UpdateWorkoutPlanCommand(
    int Id,
    string Title,
    string? Description,
    DateTime StartDate,
    DateTime? EndDate,
    bool IsActive,
    List<WorkoutExerciseItem> Exercises) : IRequest;

public class UpdateWorkoutPlanCommandHandler : IRequestHandler<UpdateWorkoutPlanCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateWorkoutPlanCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateWorkoutPlanCommand request, CancellationToken cancellationToken)
    {
        var plan = await _context.WorkoutPlans
            .Include(p => p.Exercises)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (plan is null)
        {
            throw new NotFoundException(nameof(WorkoutPlan), request.Id);
        }

        plan.Title = request.Title;
        plan.Description = request.Description;
        plan.StartDate = request.StartDate;
        plan.EndDate = request.EndDate;
        plan.IsActive = request.IsActive;
        plan.UpdatedAt = DateTime.UtcNow;

        plan.Exercises.Clear();
        foreach (var exercise in request.Exercises)
        {
            plan.Exercises.Add(new WorkoutPlanExercise
            {
                ExerciseId = exercise.ExerciseId,
                DayOfWeek = exercise.DayOfWeek,
                SetsCount = exercise.SetsCount,
                RepsCount = exercise.RepsCount,
                RestSeconds = exercise.RestSeconds,
                OrderIndex = exercise.OrderIndex
            });
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
