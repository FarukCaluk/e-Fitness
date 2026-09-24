using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using eFitness.Domain.Entities;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.WorkoutPlans.Commands.CreateWorkoutPlan;

public record CreateWorkoutPlanCommand(
    int TrainerId,
    int MemberId,
    string Title,
    string? Description,
    DateTime StartDate,
    DateTime? EndDate,
    List<WorkoutExerciseItem> Exercises) : IRequest<int>;

public class CreateWorkoutPlanCommandHandler : IRequestHandler<CreateWorkoutPlanCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateWorkoutPlanCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(CreateWorkoutPlanCommand request, CancellationToken cancellationToken)
    {
        var trainerId = request.TrainerId;

        if (_currentUserService.Role == UserRole.Trainer)
        {
            var trainer = await _context.Trainers.FirstOrDefaultAsync(t => t.UserId == _currentUserService.UserId, cancellationToken);

            if (trainer is null)
            {
                throw new ForbiddenAccessException();
            }

            trainerId = trainer.Id;
        }
        else
        {
            var trainerExists = await _context.Trainers.AnyAsync(t => t.Id == trainerId, cancellationToken);
            if (!trainerExists)
            {
                throw new NotFoundException(nameof(Trainer), trainerId);
            }
        }

        var memberExists = await _context.Members.AnyAsync(m => m.Id == request.MemberId, cancellationToken);
        if (!memberExists)
        {
            throw new NotFoundException(nameof(Member), request.MemberId);
        }

        var plan = new WorkoutPlan
        {
            TrainerId = trainerId,
            MemberId = request.MemberId,
            Title = request.Title,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            IsActive = true,
            Exercises = request.Exercises.Select(e => new WorkoutPlanExercise
            {
                ExerciseId = e.ExerciseId,
                DayOfWeek = e.DayOfWeek,
                SetsCount = e.SetsCount,
                RepsCount = e.RepsCount,
                RestSeconds = e.RestSeconds,
                OrderIndex = e.OrderIndex
            }).ToList()
        };

        _context.WorkoutPlans.Add(plan);
        await _context.SaveChangesAsync(cancellationToken);

        return plan.Id;
    }
}
