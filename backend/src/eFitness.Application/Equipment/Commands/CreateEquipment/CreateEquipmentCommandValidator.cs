using FluentValidation;

namespace eFitness.Application.Equipment.Commands.CreateEquipment;

public class CreateEquipmentCommandValidator : AbstractValidator<CreateEquipmentCommand>
{
    public CreateEquipmentCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.Category).IsInEnum();
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0);
    }
}
