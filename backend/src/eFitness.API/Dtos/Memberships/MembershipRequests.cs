using eFitness.Domain.Enums;

namespace eFitness.API.Dtos.Memberships;

public record SubscribeToMembershipRequest(int MembershipPlanId, PaymentMethod PaymentMethod, bool AutoRenew);
