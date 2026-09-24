namespace eFitness.API.Dtos.ProgressLogs;

public record CreateProgressLogRequest(decimal WeightKg, decimal? BodyFatPercentage, decimal? MuscleMassKg, string? Notes);
