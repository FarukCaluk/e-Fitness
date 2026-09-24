using eFitness.Domain.Common;

namespace eFitness.Domain.Entities;

public class WorkoutPlan : BaseAuditableEntity
{
    public int TrainerId { get; set; }
    public Trainer Trainer { get; set; } = null!;
    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<WorkoutPlanExercise> Exercises { get; set; } = new List<WorkoutPlanExercise>();
}
