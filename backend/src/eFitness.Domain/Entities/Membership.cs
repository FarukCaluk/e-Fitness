using eFitness.Domain.Common;
using eFitness.Domain.Enums;

namespace eFitness.Domain.Entities;

public class Membership : BaseAuditableEntity
{
    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;
    public int MembershipPlanId { get; set; }
    public MembershipPlan MembershipPlan { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public MembershipStatus Status { get; set; } = MembershipStatus.PendingPayment;
    public bool AutoRenew { get; set; }

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
