using eFitness.Domain.Entities;

namespace eFitness.Application.ProgressLogs;

public record ProgressLogDto(int Id, DateTime RecordedAt, decimal WeightKg, decimal? BodyFatPercentage, decimal? MuscleMassKg, string? Notes)
{
    public static ProgressLogDto FromEntity(ProgressLog log) => new(
        log.Id,
        log.RecordedAt,
        log.WeightKg,
        log.BodyFatPercentage,
        log.MuscleMassKg,
        log.Notes);
}
