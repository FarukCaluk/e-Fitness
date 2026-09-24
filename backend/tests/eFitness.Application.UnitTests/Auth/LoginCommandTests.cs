using eFitness.Application.Auth.Commands.Login;
using eFitness.Application.UnitTests.TestHelpers;
using eFitness.Domain.Entities;
using eFitness.Domain.Enums;
using eFitness.Infrastructure.Identity;
using FluentAssertions;
using Xunit;

namespace eFitness.Application.UnitTests.Auth;

public class LoginCommandTests
{
    private static User SeedUser(eFitness.Infrastructure.Persistence.ApplicationDbContext context, PasswordHasher hasher, bool isActive = true)
    {
        var user = new User
        {
            Email = "member@efitness.ba",
            PasswordHash = hasher.Hash("CorrectPassword1!"),
            FirstName = "Member",
            LastName = "One",
            Role = UserRole.Client,
            IsActive = isActive
        };

        context.Users.Add(user);
        context.SaveChanges();
        return user;
    }

    [Fact]
    public async Task Handle_ReturnsTokens_ForValidCredentials()
    {
        var context = TestDbContextFactory.Create();
        var hasher = new PasswordHasher();
        SeedUser(context, hasher);

        var handler = new LoginCommandHandler(context, hasher, TestTokenService.Create());
        var result = await handler.Handle(new LoginCommand("member@efitness.ba", "CorrectPassword1!", "127.0.0.1"), CancellationToken.None);

        result.AccessToken.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();
        result.User.Email.Should().Be("member@efitness.ba");

        context.RefreshTokens.Should().ContainSingle(rt => rt.Token == result.RefreshToken);
    }

    [Fact]
    public async Task Handle_ThrowsUnauthorized_ForWrongPassword()
    {
        var context = TestDbContextFactory.Create();
        var hasher = new PasswordHasher();
        SeedUser(context, hasher);

        var handler = new LoginCommandHandler(context, hasher, TestTokenService.Create());
        var act = () => handler.Handle(new LoginCommand("member@efitness.ba", "WrongPassword", null), CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task Handle_ThrowsUnauthorized_ForDeactivatedUser()
    {
        var context = TestDbContextFactory.Create();
        var hasher = new PasswordHasher();
        SeedUser(context, hasher, isActive: false);

        var handler = new LoginCommandHandler(context, hasher, TestTokenService.Create());
        var act = () => handler.Handle(new LoginCommand("member@efitness.ba", "CorrectPassword1!", null), CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }
}
