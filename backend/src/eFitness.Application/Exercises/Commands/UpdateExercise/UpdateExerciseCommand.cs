using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Exercises.Commands.UpdateExercise;

public record UpdateExerciseCommand(
    int Id,
    string Name,
    string? Description,
    MuscleGroup MuscleGroup,
    ExerciseDifficulty Difficulty,
    string? VideoUrl,
    int? EquipmentId) : IRequest;

public class UpdateExerciseCommandHandler : IRequestHandler<UpdateExerciseCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateExerciseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateExerciseCommand request, CancellationToken cancellationToken)
    {
        var exercise = await _context.Exercises.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (exercise is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Exercise), request.Id);
        }

        if (request.EquipmentId is not null)
        {
            var equipmentExists = await _context.Equipment.AnyAsync(e => e.Id == request.EquipmentId, cancellationToken);
            if (!equipmentExists)
            {
                throw new NotFoundException(nameof(Domain.Entities.Equipment), request.EquipmentId);
            }
        }

        exercise.Name = request.Name;
        exercise.Description = request.Description;
        exercise.MuscleGroup = request.MuscleGroup;
        exercise.Difficulty = request.Difficulty;
        exercise.VideoUrl = request.VideoUrl;
        exercise.EquipmentId = request.EquipmentId;
        exercise.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
