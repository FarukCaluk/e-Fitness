namespace eFitness.Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; set; }
}

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
