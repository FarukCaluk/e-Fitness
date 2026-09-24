using eFitness.Domain.Entities;
using eFitness.Domain.Enums;

namespace eFitness.Application.TrainingSessions;

public record TrainingSessionDto(
    int Id,
    int TrainerId,
    string TrainerName,
    int MemberId,
    string MemberName,
    DateTime ScheduledAt,
    int DurationMinutes,
    TrainingSessionStatus Status,
    string? Notes,
    string? Location)
{
    public static TrainingSessionDto FromEntity(TrainingSession session) => new(
        session.Id,
        session.TrainerId,
        $"{session.Trainer.User.FirstName} {session.Trainer.User.LastName}",
        session.MemberId,
        $"{session.Member.User.FirstName} {session.Member.User.LastName}",
        session.ScheduledAt,
        session.DurationMinutes,
        session.Status,
        session.Notes,
        session.Location);
}
