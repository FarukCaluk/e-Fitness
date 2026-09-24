using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using eFitness.Domain.Entities;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Exercises.Commands.CreateExercise;

public record CreateExerciseCommand(
    string Name,
    string? Description,
    MuscleGroup MuscleGroup,
    ExerciseDifficulty Difficulty,
    string? VideoUrl,
    int? EquipmentId) : IRequest<int>;

public class CreateExerciseCommandHandler : IRequestHandler<CreateExerciseCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateExerciseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
    {
        if (request.EquipmentId is not null)
        {
            var equipmentExists = await _context.Equipment.AnyAsync(e => e.Id == request.EquipmentId, cancellationToken);
            if (!equipmentExists)
            {
                throw new NotFoundException(nameof(Domain.Entities.Equipment), request.EquipmentId);
            }
        }

        var exercise = new Exercise
        {
            Name = request.Name,
            Description = request.Description,
            MuscleGroup = request.MuscleGroup,
            Difficulty = request.Difficulty,
            VideoUrl = request.VideoUrl,
            EquipmentId = request.EquipmentId
        };

        _context.Exercises.Add(exercise);
        await _context.SaveChangesAsync(cancellationToken);

        return exercise.Id;
    }
}
