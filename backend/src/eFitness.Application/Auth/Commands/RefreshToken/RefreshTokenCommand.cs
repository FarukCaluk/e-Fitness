using eFitness.Application.Auth.Dtos;
using eFitness.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(string Token, string? IpAddress) : IRequest<AuthResultDto>;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResultDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(IApplicationDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<AuthResultDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var existingToken = await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == request.Token, cancellationToken);

        if (existingToken is null || !existingToken.IsActive || !existingToken.User.IsActive)
        {
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");
        }

        var tokens = _tokenService.GenerateTokens(existingToken.User);

        existingToken.RevokedAt = DateTime.UtcNow;
        existingToken.ReplacedByToken = tokens.RefreshToken;

        _context.RefreshTokens.Add(new Domain.Entities.RefreshToken
        {
            UserId = existingToken.UserId,
            Token = tokens.RefreshToken,
            ExpiresAt = tokens.RefreshTokenExpiresAt,
            CreatedByIp = request.IpAddress
        });

        await _context.SaveChangesAsync(cancellationToken);

        return new AuthResultDto(
            tokens.AccessToken,
            tokens.AccessTokenExpiresAt,
            tokens.RefreshToken,
            tokens.RefreshTokenExpiresAt,
            UserSummaryDto.FromEntity(existingToken.User));
    }
}
