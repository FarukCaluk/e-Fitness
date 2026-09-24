using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Memberships.Commands.CancelMembership;

public record CancelMembershipCommand(int Id) : IRequest;

public class CancelMembershipCommandHandler : IRequestHandler<CancelMembershipCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CancelMembershipCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(CancelMembershipCommand request, CancellationToken cancellationToken)
    {
        var membership = await _context.Memberships
            .Include(m => m.Member)
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (membership is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Membership), request.Id);
        }

        if (_currentUserService.Role == UserRole.Client && membership.Member.UserId != _currentUserService.UserId)
        {
            throw new ForbiddenAccessException();
        }

        membership.Status = MembershipStatus.Cancelled;
        membership.AutoRenew = false;
        membership.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
