using FluentValidation;

namespace eFitness.Application.Exercises.Commands.UpdateExercise;

public class UpdateExerciseCommandValidator : AbstractValidator<UpdateExerciseCommand>
{
    public UpdateExerciseCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.MuscleGroup).IsInEnum();
        RuleFor(x => x.Difficulty).IsInEnum();
        RuleFor(x => x.VideoUrl).MaximumLength(500);
    }
}
