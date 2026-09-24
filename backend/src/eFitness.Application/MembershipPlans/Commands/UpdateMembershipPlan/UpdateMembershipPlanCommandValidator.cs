using FluentValidation;

namespace eFitness.Application.MembershipPlans.Commands.UpdateMembershipPlan;

public class UpdateMembershipPlanCommandValidator : AbstractValidator<UpdateMembershipPlanCommand>
{
    public UpdateMembershipPlanCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        RuleFor(x => x.DurationInDays).GreaterThan(0);
        RuleForEach(x => x.Features).NotEmpty().MaximumLength(250);
    }
}
