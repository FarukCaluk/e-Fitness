using eFitness.Domain.Common;

namespace eFitness.Domain.Entities;

public class Trainer : BaseAuditableEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public string Specialization { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public int YearsOfExperience { get; set; }
    public decimal HourlyRate { get; set; }
    public double Rating { get; set; }
    public bool IsAvailable { get; set; } = true;

    public ICollection<TrainingSession> TrainingSessions { get; set; } = new List<TrainingSession>();
    public ICollection<WorkoutPlan> WorkoutPlans { get; set; } = new List<WorkoutPlan>();
    public ICollection<Member> Clients { get; set; } = new List<Member>();
}
