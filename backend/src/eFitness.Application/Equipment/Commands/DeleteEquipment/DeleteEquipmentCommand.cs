using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Equipment.Commands.DeleteEquipment;

public record DeleteEquipmentCommand(int Id) : IRequest;

public class DeleteEquipmentCommandHandler : IRequestHandler<DeleteEquipmentCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteEquipmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteEquipmentCommand request, CancellationToken cancellationToken)
    {
        var equipment = await _context.Equipment.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (equipment is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Equipment), request.Id);
        }

        _context.Equipment.Remove(equipment);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
