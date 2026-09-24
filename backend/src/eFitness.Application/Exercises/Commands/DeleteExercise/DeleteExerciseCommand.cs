using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Exercises.Commands.DeleteExercise;

public record DeleteExerciseCommand(int Id) : IRequest;

public class DeleteExerciseCommandHandler : IRequestHandler<DeleteExerciseCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteExerciseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteExerciseCommand request, CancellationToken cancellationToken)
    {
        var exercise = await _context.Exercises.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (exercise is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Exercise), request.Id);
        }

        var isInUse = await _context.WorkoutPlanExercises.AnyAsync(wpe => wpe.ExerciseId == request.Id, cancellationToken);

        if (isInUse)
        {
            throw new ConflictException("This exercise is used in one or more workout plans and cannot be deleted.");
        }

        _context.Exercises.Remove(exercise);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
