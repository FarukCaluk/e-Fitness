using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.TrainingSessions.Commands.UpdateTrainingSessionStatus;

public record UpdateTrainingSessionStatusCommand(int Id, TrainingSessionStatus Status) : IRequest;

public class UpdateTrainingSessionStatusCommandHandler : IRequestHandler<UpdateTrainingSessionStatusCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTrainingSessionStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateTrainingSessionStatusCommand request, CancellationToken cancellationToken)
    {
        var session = await _context.TrainingSessions.FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (session is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.TrainingSession), request.Id);
        }

        session.Status = request.Status;
        session.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
