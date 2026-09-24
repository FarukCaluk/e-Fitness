namespace eFitness.Application.Common.Models;

public record TokenResult(string AccessToken, DateTime AccessTokenExpiresAt, string RefreshToken, DateTime RefreshTokenExpiresAt);
