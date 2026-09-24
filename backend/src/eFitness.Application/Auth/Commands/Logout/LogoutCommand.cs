using eFitness.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Auth.Commands.Logout;

public record LogoutCommand(string Token) : IRequest;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
{
    private readonly IApplicationDbContext _context;

    public LogoutCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == request.Token, cancellationToken);

        if (token is not null && token.IsActive)
        {
            token.RevokedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
