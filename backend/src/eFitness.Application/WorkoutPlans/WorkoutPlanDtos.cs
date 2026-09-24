using eFitness.Domain.Entities;
using eFitness.Domain.Enums;

namespace eFitness.Application.WorkoutPlans;

public record WorkoutPlanListItemDto(
    int Id,
    string Title,
    int TrainerId,
    string TrainerName,
    int MemberId,
    string MemberName,
    DateTime StartDate,
    DateTime? EndDate,
    bool IsActive)
{
    public static WorkoutPlanListItemDto FromEntity(WorkoutPlan plan) => new(
        plan.Id,
        plan.Title,
        plan.TrainerId,
        $"{plan.Trainer.User.FirstName} {plan.Trainer.User.LastName}",
        plan.MemberId,
        $"{plan.Member.User.FirstName} {plan.Member.User.LastName}",
        plan.StartDate,
        plan.EndDate,
        plan.IsActive);
}

public record WorkoutPlanExerciseDto(
    int Id,
    int ExerciseId,
    string ExerciseName,
    DayOfWeekPlan DayOfWeek,
    int SetsCount,
    int RepsCount,
    int RestSeconds,
    int OrderIndex)
{
    public static WorkoutPlanExerciseDto FromEntity(WorkoutPlanExercise entity) => new(
        entity.Id,
        entity.ExerciseId,
        entity.Exercise.Name,
        entity.DayOfWeek,
        entity.SetsCount,
        entity.RepsCount,
        entity.RestSeconds,
        entity.OrderIndex);
}

public record WorkoutPlanDetailDto(
    int Id,
    string Title,
    string? Description,
    int TrainerId,
    string TrainerName,
    int MemberId,
    string MemberName,
    DateTime StartDate,
    DateTime? EndDate,
    bool IsActive,
    List<WorkoutPlanExerciseDto> Exercises)
{
    public static WorkoutPlanDetailDto FromEntity(WorkoutPlan plan) => new(
        plan.Id,
        plan.Title,
        plan.Description,
        plan.TrainerId,
        $"{plan.Trainer.User.FirstName} {plan.Trainer.User.LastName}",
        plan.MemberId,
        $"{plan.Member.User.FirstName} {plan.Member.User.LastName}",
        plan.StartDate,
        plan.EndDate,
        plan.IsActive,
        plan.Exercises.OrderBy(e => e.DayOfWeek).ThenBy(e => e.OrderIndex).Select(WorkoutPlanExerciseDto.FromEntity).ToList());
}

public record WorkoutExerciseItem(int ExerciseId, DayOfWeekPlan DayOfWeek, int SetsCount, int RepsCount, int RestSeconds, int OrderIndex);
