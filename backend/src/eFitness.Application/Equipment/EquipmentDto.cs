using eFitness.Domain.Enums;

namespace eFitness.Application.Equipment;

public record EquipmentDto(
    int Id,
    string Name,
    string? Description,
    EquipmentCategory Category,
    int Quantity,
    bool IsAvailable)
{
    public static EquipmentDto FromEntity(Domain.Entities.Equipment equipment) => new(
        equipment.Id,
        equipment.Name,
        equipment.Description,
        equipment.Category,
        equipment.Quantity,
        equipment.IsAvailable);
}
