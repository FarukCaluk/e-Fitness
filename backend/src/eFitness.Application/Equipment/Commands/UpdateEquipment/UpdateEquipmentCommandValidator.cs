using FluentValidation;

namespace eFitness.Application.Equipment.Commands.UpdateEquipment;

public class UpdateEquipmentCommandValidator : AbstractValidator<UpdateEquipmentCommand>
{
    public UpdateEquipmentCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.Category).IsInEnum();
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0);
    }
}
