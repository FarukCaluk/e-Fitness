using eFitness.Domain.Common;
using eFitness.Domain.Enums;

namespace eFitness.Domain.Entities;

public class Exercise : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public MuscleGroup MuscleGroup { get; set; }
    public ExerciseDifficulty Difficulty { get; set; }
    public string? VideoUrl { get; set; }
    public int? EquipmentId { get; set; }
    public Equipment? Equipment { get; set; }

    public ICollection<WorkoutPlanExercise> WorkoutPlanExercises { get; set; } = new List<WorkoutPlanExercise>();
}
