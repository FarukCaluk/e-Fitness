using eFitness.Domain.Enums;

namespace eFitness.API.Dtos.Equipment;

public record CreateEquipmentRequest(string Name, string? Description, EquipmentCategory Category, int Quantity);

public record UpdateEquipmentRequest(string Name, string? Description, EquipmentCategory Category, int Quantity, bool IsAvailable);
