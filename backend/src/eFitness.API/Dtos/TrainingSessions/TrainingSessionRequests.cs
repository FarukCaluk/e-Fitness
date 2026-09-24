using eFitness.Domain.Enums;

namespace eFitness.API.Dtos.TrainingSessions;

public record CreateTrainingSessionRequest(
    int TrainerId,
    int MemberId,
    DateTime ScheduledAt,
    int DurationMinutes,
    string? Notes,
    string? Location);

public record UpdateTrainingSessionStatusRequest(TrainingSessionStatus Status);
