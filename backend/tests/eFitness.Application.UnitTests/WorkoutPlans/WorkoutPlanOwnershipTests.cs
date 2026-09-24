using eFitness.Application.UnitTests.TestHelpers;
using eFitness.Application.WorkoutPlans;
using eFitness.Application.WorkoutPlans.Commands.CreateWorkoutPlan;
using eFitness.Application.WorkoutPlans.Queries.GetWorkoutPlans;
using eFitness.Domain.Entities;
using eFitness.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace eFitness.Application.UnitTests.WorkoutPlans;

public class WorkoutPlanOwnershipTests
{
    private static (eFitness.Infrastructure.Persistence.ApplicationDbContext Context, Trainer TrainerA, Trainer TrainerB, Member Member, Exercise Exercise)
        Seed()
    {
        var context = TestDbContextFactory.Create();

        var trainerUserA = new User { Email = "ta@efitness.ba", PasswordHash = "h", FirstName = "Trainer", LastName = "A", Role = UserRole.Trainer };
        var trainerUserB = new User { Email = "tb@efitness.ba", PasswordHash = "h", FirstName = "Trainer", LastName = "B", Role = UserRole.Trainer };
        var memberUser = new User { Email = "m@efitness.ba", PasswordHash = "h", FirstName = "Member", LastName = "One", Role = UserRole.Client };
        context.Users.AddRange(trainerUserA, trainerUserB, memberUser);
        context.SaveChanges();

        var trainerA = new Trainer { UserId = trainerUserA.Id, Specialization = "Strength" };
        var trainerB = new Trainer { UserId = trainerUserB.Id, Specialization = "Yoga" };
        context.Trainers.AddRange(trainerA, trainerB);
        context.SaveChanges();

        var member = new Member { UserId = memberUser.Id, JoinDate = DateTime.UtcNow };
        context.Members.Add(member);

        var exercise = new Exercise { Name = "Squat", MuscleGroup = MuscleGroup.Legs, Difficulty = ExerciseDifficulty.Intermediate };
        context.Exercises.Add(exercise);
        context.SaveChanges();

        return (context, trainerA, trainerB, member, exercise);
    }

    [Fact]
    public async Task Create_IgnoresClientSuppliedTrainerId_AndUsesCallersOwnTrainerProfile()
    {
        var (context, trainerA, trainerB, member, exercise) = Seed();

        var currentUser = new FakeCurrentUserService { UserId = trainerA.UserId, Role = UserRole.Trainer };
        var handler = new CreateWorkoutPlanCommandHandler(context, currentUser);

        var exercises = new List<WorkoutExerciseItem> { new(exercise.Id, DayOfWeekPlan.Monday, 3, 10, 60, 0) };

        var planId = await handler.Handle(
            new CreateWorkoutPlanCommand(trainerB.Id, member.Id, "Plan", null, DateTime.UtcNow, null, exercises),
            CancellationToken.None);

        var plan = context.WorkoutPlans.Single(p => p.Id == planId);
        plan.TrainerId.Should().Be(trainerA.Id);
    }

    [Fact]
    public async Task GetPlans_ForClientRole_OnlyReturnsTheirOwnPlans()
    {
        var (context, trainerA, _, member, _) = Seed();

        var otherMemberUser = new User { Email = "other@efitness.ba", PasswordHash = "h", FirstName = "Other", LastName = "Member", Role = UserRole.Client };
        context.Users.Add(otherMemberUser);
        context.SaveChanges();
        var otherMember = new Member { UserId = otherMemberUser.Id, JoinDate = DateTime.UtcNow };
        context.Members.Add(otherMember);
        context.SaveChanges();

        context.WorkoutPlans.Add(new Domain.Entities.WorkoutPlan { TrainerId = trainerA.Id, MemberId = member.Id, Title = "Mine", StartDate = DateTime.UtcNow, IsActive = true });
        context.WorkoutPlans.Add(new Domain.Entities.WorkoutPlan { TrainerId = trainerA.Id, MemberId = otherMember.Id, Title = "NotMine", StartDate = DateTime.UtcNow, IsActive = true });
        context.SaveChanges();

        var currentUser = new FakeCurrentUserService { UserId = member.UserId, Role = UserRole.Client };
        var handler = new GetWorkoutPlansQueryHandler(context, currentUser);

        var result = await handler.Handle(new GetWorkoutPlansQuery(1, 10, null, null, null), CancellationToken.None);

        result.TotalCount.Should().Be(1);
        result.Items.Single().Title.Should().Be("Mine");
    }
}
