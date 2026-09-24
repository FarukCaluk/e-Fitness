using eFitness.Application.Common.Interfaces;
using eFitness.Domain.Enums;
using MediatR;

namespace eFitness.Application.Equipment.Commands.CreateEquipment;

public record CreateEquipmentCommand(string Name, string? Description, EquipmentCategory Category, int Quantity) : IRequest<int>;

public class CreateEquipmentCommandHandler : IRequestHandler<CreateEquipmentCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateEquipmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateEquipmentCommand request, CancellationToken cancellationToken)
    {
        var equipment = new Domain.Entities.Equipment
        {
            Name = request.Name,
            Description = request.Description,
            Category = request.Category,
            Quantity = request.Quantity,
            IsAvailable = request.Quantity > 0
        };

        _context.Equipment.Add(equipment);
        await _context.SaveChangesAsync(cancellationToken);

        return equipment.Id;
    }
}
