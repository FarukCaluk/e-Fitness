using eFitness.Domain.Enums;

namespace eFitness.API.Dtos.Exercises;

public record CreateExerciseRequest(
    string Name,
    string? Description,
    MuscleGroup MuscleGroup,
    ExerciseDifficulty Difficulty,
    string? VideoUrl,
    int? EquipmentId);

public record UpdateExerciseRequest(
    string Name,
    string? Description,
    MuscleGroup MuscleGroup,
    ExerciseDifficulty Difficulty,
    string? VideoUrl,
    int? EquipmentId);
