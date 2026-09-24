using FluentValidation;

namespace eFitness.Application.Exercises.Commands.CreateExercise;

public class CreateExerciseCommandValidator : AbstractValidator<CreateExerciseCommand>
{
    public CreateExerciseCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.MuscleGroup).IsInEnum();
        RuleFor(x => x.Difficulty).IsInEnum();
        RuleFor(x => x.VideoUrl).MaximumLength(500);
    }
}
