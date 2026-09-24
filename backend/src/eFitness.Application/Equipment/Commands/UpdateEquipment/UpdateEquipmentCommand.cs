using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Equipment.Commands.UpdateEquipment;

public record UpdateEquipmentCommand(
    int Id,
    string Name,
    string? Description,
    EquipmentCategory Category,
    int Quantity,
    bool IsAvailable) : IRequest;

public class UpdateEquipmentCommandHandler : IRequestHandler<UpdateEquipmentCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateEquipmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateEquipmentCommand request, CancellationToken cancellationToken)
    {
        var equipment = await _context.Equipment.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (equipment is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Equipment), request.Id);
        }

        equipment.Name = request.Name;
        equipment.Description = request.Description;
        equipment.Category = request.Category;
        equipment.Quantity = request.Quantity;
        equipment.IsAvailable = request.IsAvailable;
        equipment.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
