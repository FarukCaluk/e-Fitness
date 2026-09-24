using eFitness.Application.Common.Exceptions;
using eFitness.Application.TrainingSessions.Commands.CreateTrainingSession;
using eFitness.Application.TrainingSessions.Queries.GetTrainingSessions;
using eFitness.Application.UnitTests.TestHelpers;
using eFitness.Domain.Entities;
using eFitness.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace eFitness.Application.UnitTests.TrainingSessions;

public class TrainingSessionOwnershipTests
{
    private static (eFitness.Infrastructure.Persistence.ApplicationDbContext Context, Trainer TrainerA, Trainer TrainerB, Member Member)
        SeedTwoTrainersAndAMember()
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
        context.SaveChanges();

        return (context, trainerA, trainerB, member);
    }

    [Fact]
    public async Task CreateSession_IgnoresClientSuppliedTrainerId_AndUsesCallersOwnTrainerProfile()
    {
        var (context, trainerA, trainerB, member) = SeedTwoTrainersAndAMember();

        var currentUser = new FakeCurrentUserService { UserId = trainerA.UserId, Role = UserRole.Trainer };
        var handler = new CreateTrainingSessionCommandHandler(context, currentUser);

        var sessionId = await handler.Handle(
            new CreateTrainingSessionCommand(trainerB.Id, member.Id, DateTime.UtcNow.AddDays(1), 60, null, null),
            CancellationToken.None);

        var session = context.TrainingSessions.Single(s => s.Id == sessionId);
        session.TrainerId.Should().Be(trainerA.Id, "the acting trainer is resolved from the JWT, not the request body");
        session.TrainerId.Should().NotBe(trainerB.Id);
    }

    [Fact]
    public async Task GetSessions_ForTrainerRole_OnlyReturnsTheirOwnSessions()
    {
        var (context, trainerA, trainerB, member) = SeedTwoTrainersAndAMember();

        context.TrainingSessions.Add(new TrainingSession { TrainerId = trainerA.Id, MemberId = member.Id, ScheduledAt = DateTime.UtcNow.AddDays(1), DurationMinutes = 60 });
        context.TrainingSessions.Add(new TrainingSession { TrainerId = trainerB.Id, MemberId = member.Id, ScheduledAt = DateTime.UtcNow.AddDays(1), DurationMinutes = 60 });
        context.SaveChanges();

        var currentUser = new FakeCurrentUserService { UserId = trainerA.UserId, Role = UserRole.Trainer };
        var handler = new GetTrainingSessionsQueryHandler(context, currentUser);

        var result = await handler.Handle(new GetTrainingSessionsQuery(1, 10, null, null, null, null, null), CancellationToken.None);

        result.TotalCount.Should().Be(1);
        result.Items.Single().TrainerId.Should().Be(trainerA.Id);
    }
}
