using FluentValidation;

namespace eFitness.Application.TrainingSessions.Commands.CreateTrainingSession;

public class CreateTrainingSessionCommandValidator : AbstractValidator<CreateTrainingSessionCommand>
{
    public CreateTrainingSessionCommandValidator()
    {
        RuleFor(x => x.TrainerId).GreaterThan(0);
        RuleFor(x => x.MemberId).GreaterThan(0);
        RuleFor(x => x.ScheduledAt).GreaterThan(DateTime.UtcNow);
        RuleFor(x => x.DurationMinutes).InclusiveBetween(15, 240);
        RuleFor(x => x.Notes).MaximumLength(1000);
        RuleFor(x => x.Location).MaximumLength(150);
    }
}
