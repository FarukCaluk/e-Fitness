using eFitness.Application.Auth.Commands.Register;
using eFitness.Application.Common.Exceptions;
using eFitness.Application.UnitTests.TestHelpers;
using eFitness.Domain.Enums;
using eFitness.Infrastructure.Identity;
using FluentAssertions;
using Xunit;

namespace eFitness.Application.UnitTests.Auth;

public class RegisterCommandTests
{
    [Fact]
    public async Task Handle_CreatesUserAndMemberProfile_WithClientRole()
    {
        var context = TestDbContextFactory.Create();
        var handler = new RegisterCommandHandler(context, new PasswordHasher());

        var command = new RegisterCommand("new.client@efitness.ba", "Password123!", "New", "Client", "061000000");

        var userId = await handler.Handle(command, CancellationToken.None);

        var user = context.Users.Single(u => u.Id == userId);
        user.Role.Should().Be(UserRole.Client);
        user.Email.Should().Be("new.client@efitness.ba");
        user.PasswordHash.Should().NotBe("Password123!");

        var member = context.Members.SingleOrDefault(m => m.UserId == userId);
        member.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_ThrowsConflict_WhenEmailAlreadyRegistered()
    {
        var context = TestDbContextFactory.Create();
        var hasher = new PasswordHasher();
        var handler = new RegisterCommandHandler(context, hasher);

        var command = new RegisterCommand("dup@efitness.ba", "Password123!", "First", "User", null);
        await handler.Handle(command, CancellationToken.None);

        var act = () => handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ConflictException>();
    }
}
