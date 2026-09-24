namespace eFitness.Application.Auth.Dtos;

public record AuthResultDto(
    string AccessToken,
    DateTime AccessTokenExpiresAt,
    string RefreshToken,
    DateTime RefreshTokenExpiresAt,
    UserSummaryDto User);
