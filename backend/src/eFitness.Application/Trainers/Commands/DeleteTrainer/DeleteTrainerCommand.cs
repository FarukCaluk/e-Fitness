using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Trainers.Commands.DeleteTrainer;

public record DeleteTrainerCommand(int Id) : IRequest;

public class DeleteTrainerCommandHandler : IRequestHandler<DeleteTrainerCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteTrainerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteTrainerCommand request, CancellationToken cancellationToken)
    {
        var trainer = await _context.Trainers
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (trainer is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Trainer), request.Id);
        }

        trainer.IsAvailable = false;
        trainer.User.IsActive = false;
        trainer.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
