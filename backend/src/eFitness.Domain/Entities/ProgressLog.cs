using eFitness.Domain.Common;

namespace eFitness.Domain.Entities;

public class ProgressLog : BaseEntity
{
    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;
    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
    public decimal WeightKg { get; set; }
    public decimal? BodyFatPercentage { get; set; }
    public decimal? MuscleMassKg { get; set; }
    public string? Notes { get; set; }
}
