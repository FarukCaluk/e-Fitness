using FluentValidation;

namespace eFitness.Application.Trainers.Commands.UpdateTrainer;

public class UpdateTrainerCommandValidator : AbstractValidator<UpdateTrainerCommand>
{
    public UpdateTrainerCommandValidator()
    {
        RuleFor(x => x.Specialization).NotEmpty().MaximumLength(150);
        RuleFor(x => x.YearsOfExperience).GreaterThanOrEqualTo(0).LessThanOrEqualTo(80);
        RuleFor(x => x.HourlyRate).GreaterThanOrEqualTo(0);
    }
}
