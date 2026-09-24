using eFitness.Application.Auth.Dtos;
using eFitness.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Auth.Commands.Login;

public record LoginCommand(string Email, string Password, string? IpAddress) : IRequest<AuthResultDto>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResultDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<AuthResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user is null || !user.IsActive || !_passwordHasher.Verify(user.PasswordHash, request.Password))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var tokens = _tokenService.GenerateTokens(user);

        _context.RefreshTokens.Add(new Domain.Entities.RefreshToken
        {
            UserId = user.Id,
            Token = tokens.RefreshToken,
            ExpiresAt = tokens.RefreshTokenExpiresAt,
            CreatedByIp = request.IpAddress
        });

        user.LastLoginAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new AuthResultDto(
            tokens.AccessToken,
            tokens.AccessTokenExpiresAt,
            tokens.RefreshToken,
            tokens.RefreshTokenExpiresAt,
            UserSummaryDto.FromEntity(user));
    }
}
