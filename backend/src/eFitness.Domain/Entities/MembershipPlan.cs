using eFitness.Domain.Common;

namespace eFitness.Domain.Entities;

public class MembershipPlan : BaseAuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int DurationInDays { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<MembershipPlanFeature> Features { get; set; } = new List<MembershipPlanFeature>();
    public ICollection<Membership> Memberships { get; set; } = new List<Membership>();
}

public class MembershipPlanFeature : BaseEntity
{
    public int MembershipPlanId { get; set; }
    public MembershipPlan MembershipPlan { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
}
