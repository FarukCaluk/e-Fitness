using FluentValidation;

namespace eFitness.Application.ProgressLogs.Commands.CreateProgressLog;

public class CreateProgressLogCommandValidator : AbstractValidator<CreateProgressLogCommand>
{
    public CreateProgressLogCommandValidator()
    {
        RuleFor(x => x.WeightKg).InclusiveBetween(1, 500);
        RuleFor(x => x.BodyFatPercentage).InclusiveBetween(0, 100).When(x => x.BodyFatPercentage is not null);
        RuleFor(x => x.MuscleMassKg).InclusiveBetween(0, 300).When(x => x.MuscleMassKg is not null);
        RuleFor(x => x.Notes).MaximumLength(500);
    }
}
