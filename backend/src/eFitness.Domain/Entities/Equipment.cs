using eFitness.Domain.Common;
using eFitness.Domain.Enums;

namespace eFitness.Domain.Entities;

public class Equipment : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public EquipmentCategory Category { get; set; }
    public int Quantity { get; set; }
    public bool IsAvailable { get; set; } = true;
    public DateTime? LastMaintenanceAt { get; set; }

    public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
}
