using FluentValidation;

namespace eFitness.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        RuleFor(x => x.StockQuantity).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ImageUrl).MaximumLength(500);
    }
}
