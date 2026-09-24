using eFitness.Domain.Common;
using eFitness.Domain.Enums;

namespace eFitness.Domain.Entities;

public class TrainingSession : BaseAuditableEntity
{
    public int TrainerId { get; set; }
    public Trainer Trainer { get; set; } = null!;
    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;
    public DateTime ScheduledAt { get; set; }
    public int DurationMinutes { get; set; } = 60;
    public TrainingSessionStatus Status { get; set; } = TrainingSessionStatus.Scheduled;
    public string? Notes { get; set; }
    public string? Location { get; set; }
}
