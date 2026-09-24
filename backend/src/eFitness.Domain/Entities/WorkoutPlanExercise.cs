using eFitness.Domain.Common;
using eFitness.Domain.Enums;

namespace eFitness.Domain.Entities;

public class WorkoutPlanExercise : BaseEntity
{
    public int WorkoutPlanId { get; set; }
    public WorkoutPlan WorkoutPlan { get; set; } = null!;
    public int ExerciseId { get; set; }
    public Exercise Exercise { get; set; } = null!;
    public DayOfWeekPlan DayOfWeek { get; set; }
    public int SetsCount { get; set; }
    public int RepsCount { get; set; }
    public int RestSeconds { get; set; }
    public int OrderIndex { get; set; }
}
