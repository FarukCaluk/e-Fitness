using eFitness.Domain.Entities;
using eFitness.Domain.Enums;

namespace eFitness.Application.Exercises;

public record ExerciseDto(
    int Id,
    string Name,
    string? Description,
    MuscleGroup MuscleGroup,
    ExerciseDifficulty Difficulty,
    string? VideoUrl,
    int? EquipmentId,
    string? EquipmentName)
{
    public static ExerciseDto FromEntity(Exercise exercise) => new(
        exercise.Id,
        exercise.Name,
        exercise.Description,
        exercise.MuscleGroup,
        exercise.Difficulty,
        exercise.VideoUrl,
        exercise.EquipmentId,
        exercise.Equipment?.Name);
}
