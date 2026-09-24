using eFitness.Application.Common.Exceptions;
using eFitness.Application.Trainers.Commands.DeleteTrainer;
using eFitness.Application.UnitTests.TestHelpers;
using eFitness.Domain.Entities;
using eFitness.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace eFitness.Application.UnitTests.Trainers;

public class DeleteTrainerCommandTests
{
    [Fact]
    public async Task Handle_DeactivatesTrainerAndUser_RatherThanDeleting()
    {
        var context = TestDbContextFactory.Create();
        var user = new User { Email = "t@efitness.ba", PasswordHash = "hash", FirstName = "T", LastName = "One", Role = UserRole.Trainer, IsActive = true };
        context.Users.Add(user);
        context.SaveChanges();

        var trainer = new Trainer { UserId = user.Id, Specialization = "Yoga", IsAvailable = true };
        context.Trainers.Add(trainer);
        context.SaveChanges();

        var handler = new DeleteTrainerCommandHandler(context);
        await handler.Handle(new DeleteTrainerCommand(trainer.Id), CancellationToken.None);

        context.Trainers.Should().ContainSingle(t => t.Id == trainer.Id);
        context.Trainers.Single(t => t.Id == trainer.Id).IsAvailable.Should().BeFalse();
        context.Users.Single(u => u.Id == user.Id).IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ThrowsNotFound_ForUnknownTrainer()
    {
        var context = TestDbContextFactory.Create();
        var handler = new DeleteTrainerCommandHandler(context);

        var act = () => handler.Handle(new DeleteTrainerCommand(999), CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
