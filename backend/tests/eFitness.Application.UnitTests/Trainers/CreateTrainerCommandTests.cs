using eFitness.Application.Common.Exceptions;
using eFitness.Application.Trainers.Commands.CreateTrainer;
using eFitness.Application.UnitTests.TestHelpers;
using eFitness.Domain.Enums;
using eFitness.Infrastructure.Identity;
using FluentAssertions;
using Xunit;

namespace eFitness.Application.UnitTests.Trainers;

public class CreateTrainerCommandTests
{
    [Fact]
    public async Task Handle_CreatesUserAndTrainerProfile_WithTrainerRole()
    {
        var context = TestDbContextFactory.Create();
        var handler = new CreateTrainerCommandHandler(context, new PasswordHasher());

        var command = new CreateTrainerCommand(
            "trener@efitness.ba", "Password123!", "Amar", "Kovac", "061000000",
            "Strength & Conditioning", "Bio text", 5, 40m);

        var trainerId = await handler.Handle(command, CancellationToken.None);

        var trainer = context.Trainers.Single(t => t.Id == trainerId);
        var user = context.Users.Single(u => u.Id == trainer.UserId);

        user.Role.Should().Be(UserRole.Trainer);
        trainer.Specialization.Should().Be("Strength & Conditioning");
        trainer.IsAvailable.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ThrowsConflict_WhenEmailAlreadyExists()
    {
        var context = TestDbContextFactory.Create();
        var hasher = new PasswordHasher();
        var handler = new CreateTrainerCommandHandler(context, hasher);

        var command = new CreateTrainerCommand(
            "dup-trainer@efitness.ba", "Password123!", "A", "B", null, "Yoga", null, 2, 30m);

        await handler.Handle(command, CancellationToken.None);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ConflictException>();
    }
}
