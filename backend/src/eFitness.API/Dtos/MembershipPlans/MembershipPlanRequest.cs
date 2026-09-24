namespace eFitness.API.Dtos.MembershipPlans;

public record CreateMembershipPlanRequest(
    string Name,
    string? Description,
    decimal Price,
    int DurationInDays,
    bool IsFeatured,
    List<string> Features);

public record UpdateMembershipPlanRequest(
    string Name,
    string? Description,
    decimal Price,
    int DurationInDays,
    bool IsFeatured,
    bool IsActive,
    List<string> Features);
