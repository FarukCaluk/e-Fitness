using eFitness.Application.Common.Models;
using eFitness.Domain.Entities;

namespace eFitness.Application.Common.Interfaces;

public interface ITokenService
{
    TokenResult GenerateTokens(User user);
    string GenerateRefreshTokenValue();
}
