using FluentValidation;

namespace eFitness.Application.Trainers.Commands.CreateTrainer;

public class CreateTrainerCommandValidator : AbstractValidator<CreateTrainerCommand>
{
    public CreateTrainerCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(128);
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Specialization).NotEmpty().MaximumLength(150);
        RuleFor(x => x.YearsOfExperience).GreaterThanOrEqualTo(0).LessThanOrEqualTo(80);
        RuleFor(x => x.HourlyRate).GreaterThanOrEqualTo(0);
    }
}
