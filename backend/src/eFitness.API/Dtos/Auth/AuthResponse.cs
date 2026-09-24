using eFitness.Application.Auth.Dtos;

namespace eFitness.API.Dtos.Auth;

public record AuthResponse(string AccessToken, DateTime AccessTokenExpiresAt, UserSummaryDto User)
{
    public static AuthResponse FromResult(AuthResultDto result) =>
        new(result.AccessToken, result.AccessTokenExpiresAt, result.User);
}
