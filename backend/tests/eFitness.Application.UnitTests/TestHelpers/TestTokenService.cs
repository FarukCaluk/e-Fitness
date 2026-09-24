using eFitness.Infrastructure.Identity;
using Microsoft.Extensions.Options;

namespace eFitness.Application.UnitTests.TestHelpers;

public static class TestTokenService
{
    public static TokenService Create()
    {
        var settings = new JwtSettings
        {
            Secret = "unit-test-secret-key-at-least-32-characters-long",
            Issuer = "eFitness.Tests",
            Audience = "eFitness.Tests",
            AccessTokenExpirationMinutes = 15,
            RefreshTokenExpirationDays = 7
        };

        return new TokenService(Options.Create(settings));
    }
}
