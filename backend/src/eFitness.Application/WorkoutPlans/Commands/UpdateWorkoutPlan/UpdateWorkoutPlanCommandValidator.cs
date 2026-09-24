using FluentValidation;

namespace eFitness.Application.WorkoutPlans.Commands.UpdateWorkoutPlan;

public class UpdateWorkoutPlanCommandValidator : AbstractValidator<UpdateWorkoutPlanCommand>
{
    public UpdateWorkoutPlanCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.Exercises).NotEmpty();

        RuleForEach(x => x.Exercises).ChildRules(exercise =>
        {
            exercise.RuleFor(e => e.ExerciseId).GreaterThan(0);
            exercise.RuleFor(e => e.SetsCount).InclusiveBetween(1, 20);
            exercise.RuleFor(e => e.RepsCount).InclusiveBetween(1, 100);
            exercise.RuleFor(e => e.RestSeconds).InclusiveBetween(0, 600);
        });
    }
}
