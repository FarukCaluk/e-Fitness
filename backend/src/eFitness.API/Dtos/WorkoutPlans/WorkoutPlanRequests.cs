using eFitness.Application.WorkoutPlans;

namespace eFitness.API.Dtos.WorkoutPlans;

public record CreateWorkoutPlanRequest(
    int TrainerId,
    int MemberId,
    string Title,
    string? Description,
    DateTime StartDate,
    DateTime? EndDate,
    List<WorkoutExerciseItem> Exercises);

public record UpdateWorkoutPlanRequest(
    string Title,
    string? Description,
    DateTime StartDate,
    DateTime? EndDate,
    bool IsActive,
    List<WorkoutExerciseItem> Exercises);
