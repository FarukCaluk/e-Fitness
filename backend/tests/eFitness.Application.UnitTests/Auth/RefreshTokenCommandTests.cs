using eFitness.Application.Auth.Commands.RefreshToken;
using eFitness.Application.UnitTests.TestHelpers;
using eFitness.Domain.Entities;
using eFitness.Domain.Enums;
using FluentAssertions;
using Xunit;

namespace eFitness.Application.UnitTests.Auth;

public class RefreshTokenCommandTests
{
    [Fact]
    public async Task Handle_RotatesToken_AndRevokesOldOne()
    {
        var context = TestDbContextFactory.Create();
        var user = new User
        {
            Email = "rotate@efitness.ba",
            PasswordHash = "hash",
            FirstName = "R",
            LastName = "User",
            Role = UserRole.Client,
            IsActive = true
        };
        context.Users.Add(user);
        context.SaveChanges();

        var oldToken = new RefreshToken
        {
            UserId = user.Id,
            Token = "old-token-value",
            ExpiresAt = DateTime.UtcNow.AddDays(1)
        };
        context.RefreshTokens.Add(oldToken);
        context.SaveChanges();

        var handler = new RefreshTokenCommandHandler(context, TestTokenService.Create());
        var result = await handler.Handle(new RefreshTokenCommand("old-token-value", "127.0.0.1"), CancellationToken.None);

        result.RefreshToken.Should().NotBe("old-token-value");

        var reloadedOldToken = context.RefreshTokens.Single(rt => rt.Token == "old-token-value");
        reloadedOldToken.IsActive.Should().BeFalse();
        reloadedOldToken.ReplacedByToken.Should().Be(result.RefreshToken);

        context.RefreshTokens.Should().ContainSingle(rt => rt.Token == result.RefreshToken);
    }

    [Fact]
    public async Task Handle_ThrowsUnauthorized_ForUnknownToken()
    {
        var context = TestDbContextFactory.Create();
        var handler = new RefreshTokenCommandHandler(context, TestTokenService.Create());

        var act = () => handler.Handle(new RefreshTokenCommand("does-not-exist", null), CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task Handle_ThrowsUnauthorized_ForExpiredToken()
    {
        var context = TestDbContextFactory.Create();
        var user = new User
        {
            Email = "expired@efitness.ba",
            PasswordHash = "hash",
            FirstName = "E",
            LastName = "User",
            Role = UserRole.Client,
            IsActive = true
        };
        context.Users.Add(user);
        context.SaveChanges();

        context.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            Token = "expired-token",
            ExpiresAt = DateTime.UtcNow.AddDays(-1)
        });
        context.SaveChanges();

        var handler = new RefreshTokenCommandHandler(context, TestTokenService.Create());
        var act = () => handler.Handle(new RefreshTokenCommand("expired-token", null), CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }
}
