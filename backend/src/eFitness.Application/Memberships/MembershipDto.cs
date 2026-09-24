using eFitness.Domain.Entities;
using eFitness.Domain.Enums;

namespace eFitness.Application.Memberships;

public record MembershipDto(
    int Id,
    int MemberId,
    string MemberName,
    int MembershipPlanId,
    string PlanName,
    DateTime StartDate,
    DateTime EndDate,
    MembershipStatus Status,
    bool AutoRenew)
{
    public static MembershipDto FromEntity(Membership membership) => new(
        membership.Id,
        membership.MemberId,
        $"{membership.Member.User.FirstName} {membership.Member.User.LastName}",
        membership.MembershipPlanId,
        membership.MembershipPlan.Name,
        membership.StartDate,
        membership.EndDate,
        membership.Status,
        membership.AutoRenew);
}
