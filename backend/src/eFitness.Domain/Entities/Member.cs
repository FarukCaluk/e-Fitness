using eFitness.Domain.Common;

namespace eFitness.Domain.Entities;

public class Member : BaseAuditableEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public DateTime JoinDate { get; set; } = DateTime.UtcNow;
    public int? AssignedTrainerId { get; set; }
    public Trainer? AssignedTrainer { get; set; }

    public ICollection<Membership> Memberships { get; set; } = new List<Membership>();
    public ICollection<TrainingSession> TrainingSessions { get; set; } = new List<TrainingSession>();
    public ICollection<WorkoutPlan> WorkoutPlans { get; set; } = new List<WorkoutPlan>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<ProgressLog> ProgressLogs { get; set; } = new List<ProgressLog>();
}
