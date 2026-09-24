using eFitness.Domain.Common;
using eFitness.Domain.Enums;

namespace eFitness.Domain.Entities;

public class Payment : BaseAuditableEntity
{
    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public PaymentMethod Method { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public PaymentPurpose Purpose { get; set; }
    public string? TransactionReference { get; set; }

    public int? MembershipId { get; set; }
    public Membership? Membership { get; set; }

    public int? OrderId { get; set; }
    public Order? Order { get; set; }
}
