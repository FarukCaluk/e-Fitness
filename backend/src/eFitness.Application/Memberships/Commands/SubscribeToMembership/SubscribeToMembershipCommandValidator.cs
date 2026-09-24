using FluentValidation;

namespace eFitness.Application.Memberships.Commands.SubscribeToMembership;

public class SubscribeToMembershipCommandValidator : AbstractValidator<SubscribeToMembershipCommand>
{
    public SubscribeToMembershipCommandValidator()
    {
        RuleFor(x => x.MembershipPlanId).GreaterThan(0);
        RuleFor(x => x.PaymentMethod).IsInEnum();
    }
}
