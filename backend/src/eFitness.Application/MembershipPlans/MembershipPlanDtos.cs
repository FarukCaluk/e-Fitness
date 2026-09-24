using eFitness.Domain.Entities;

namespace eFitness.Application.MembershipPlans;

public record MembershipPlanDto(
    int Id,
    string Name,
    string? Description,
    decimal Price,
    int DurationInDays,
    bool IsFeatured,
    bool IsActive,
    List<string> Features)
{
    public static MembershipPlanDto FromEntity(MembershipPlan plan) => new(
        plan.Id,
        plan.Name,
        plan.Description,
        plan.Price,
        plan.DurationInDays,
        plan.IsFeatured,
        plan.IsActive,
        plan.Features.OrderBy(f => f.OrderIndex).Select(f => f.Description).ToList());
}
