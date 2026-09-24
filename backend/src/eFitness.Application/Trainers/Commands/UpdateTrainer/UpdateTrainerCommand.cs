using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Trainers.Commands.UpdateTrainer;

public record UpdateTrainerCommand(
    int Id,
    string Specialization,
    string? Bio,
    int YearsOfExperience,
    decimal HourlyRate,
    bool IsAvailable) : IRequest;

public class UpdateTrainerCommandHandler : IRequestHandler<UpdateTrainerCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTrainerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateTrainerCommand request, CancellationToken cancellationToken)
    {
        var trainer = await _context.Trainers.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (trainer is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Trainer), request.Id);
        }

        trainer.Specialization = request.Specialization;
        trainer.Bio = request.Bio;
        trainer.YearsOfExperience = request.YearsOfExperience;
        trainer.HourlyRate = request.HourlyRate;
        trainer.IsAvailable = request.IsAvailable;
        trainer.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
