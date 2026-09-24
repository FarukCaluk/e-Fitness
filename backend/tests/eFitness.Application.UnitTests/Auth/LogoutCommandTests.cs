using eFitness.Application.Auth.Commands.Logout;
using eFitness.Application.UnitTests.TestHelpers;
using eFitness.Domain.Entities;
using eFitness.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace eFitness.Application.UnitTests.Auth;

public class LogoutCommandTests
{
    [Fact]
    public async Task Handle_RevokesTheGivenToken()
    {
        var context = TestDbContextFactory.Create();
        var user = new User
        {
            Email = "logout@efitness.ba",
            PasswordHash = "hash",
            FirstName = "L",
            LastName = "User",
            Role = UserRole.Client,
            IsActive = true
        };
        context.Users.Add(user);
        context.SaveChanges();

        context.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            Token = "token-to-revoke",
            ExpiresAt = DateTime.UtcNow.AddDays(1)
        });
        context.SaveChanges();

        var handler = new LogoutCommandHandler(context);
        await handler.Handle(new LogoutCommand("token-to-revoke"), CancellationToken.None);

        var token = context.RefreshTokens.Single(rt => rt.Token == "token-to-revoke");
        token.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_DoesNothing_ForUnknownToken()
    {
        var context = TestDbContextFactory.Create();
        var handler = new LogoutCommandHandler(context);

        var act = () => handler.Handle(new LogoutCommand("unknown"), CancellationToken.None);

        await act.Should().NotThrowAsync();
    }
}
