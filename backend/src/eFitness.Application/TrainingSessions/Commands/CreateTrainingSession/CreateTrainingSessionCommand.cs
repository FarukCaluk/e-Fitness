using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using eFitness.Domain.Entities;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.TrainingSessions.Commands.CreateTrainingSession;

public record CreateTrainingSessionCommand(
    int TrainerId,
    int MemberId,
    DateTime ScheduledAt,
    int DurationMinutes,
    string? Notes,
    string? Location) : IRequest<int>;

public class CreateTrainingSessionCommandHandler : IRequestHandler<CreateTrainingSessionCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateTrainingSessionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateTrainingSessionCommand request, CancellationToken cancellationToken)
    {
        var trainerExists = await _context.Trainers.AnyAsync(t => t.Id == request.TrainerId, cancellationToken);
        if (!trainerExists)
        {
            throw new NotFoundException(nameof(Trainer), request.TrainerId);
        }

        var memberExists = await _context.Members.AnyAsync(m => m.Id == request.MemberId, cancellationToken);
        if (!memberExists)
        {
            throw new NotFoundException(nameof(Member), request.MemberId);
        }

        var session = new TrainingSession
        {
            TrainerId = request.TrainerId,
            MemberId = request.MemberId,
            ScheduledAt = request.ScheduledAt,
            DurationMinutes = request.DurationMinutes,
            Notes = request.Notes,
            Location = request.Location,
            Status = TrainingSessionStatus.Scheduled
        };

        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync(cancellationToken);

        return session.Id;
    }
}
