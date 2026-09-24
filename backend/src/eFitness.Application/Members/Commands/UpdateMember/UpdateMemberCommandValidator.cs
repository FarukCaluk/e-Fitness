using FluentValidation;

namespace eFitness.Application.Members.Commands.UpdateMember;

public class UpdateMemberCommandValidator : AbstractValidator<UpdateMemberCommand>
{
    public UpdateMemberCommandValidator()
    {
        RuleFor(x => x.PhoneNumber).MaximumLength(30);
        RuleFor(x => x.Gender).MaximumLength(20);
        RuleFor(x => x.Address).MaximumLength(250);
        RuleFor(x => x.City).MaximumLength(100);
        RuleFor(x => x.EmergencyContactName).MaximumLength(150);
        RuleFor(x => x.EmergencyContactPhone).MaximumLength(30);
    }
}
