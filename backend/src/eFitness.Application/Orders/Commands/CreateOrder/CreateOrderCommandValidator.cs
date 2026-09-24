using FluentValidation;

namespace eFitness.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Items).NotEmpty();
        RuleFor(x => x.ShippingAddress).MaximumLength(250);
        RuleFor(x => x.PaymentMethod).IsInEnum();

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductId).GreaterThan(0);
            item.RuleFor(i => i.Quantity).GreaterThan(0);
        });
    }
}
