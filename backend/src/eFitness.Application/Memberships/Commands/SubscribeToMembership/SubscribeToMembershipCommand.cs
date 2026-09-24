using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using eFitness.Domain.Entities;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Memberships.Commands.SubscribeToMembership;

public record SubscribeToMembershipCommand(int MembershipPlanId, PaymentMethod PaymentMethod, bool AutoRenew) : IRequest<int>;

public class SubscribeToMembershipCommandHandler : IRequestHandler<SubscribeToMembershipCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public SubscribeToMembershipCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(SubscribeToMembershipCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is null)
        {
            throw new ForbiddenAccessException();
        }

        var member = await _context.Members.FirstOrDefaultAsync(m => m.UserId == _currentUserService.UserId, cancellationToken);

        if (member is null)
        {
            throw new ForbiddenAccessException();
        }

        var plan = await _context.MembershipPlans.FirstOrDefaultAsync(p => p.Id == request.MembershipPlanId && p.IsActive, cancellationToken);

        if (plan is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.MembershipPlan), request.MembershipPlanId);
        }

        var activeMemberships = await _context.Memberships
            .Where(m => m.MemberId == member.Id && m.Status == MembershipStatus.Active)
            .ToListAsync(cancellationToken);

        foreach (var active in activeMemberships)
        {
            active.Status = MembershipStatus.Cancelled;
            active.UpdatedAt = DateTime.UtcNow;
        }

        var startDate = DateTime.UtcNow;

        var membership = new Membership
        {
            MemberId = member.Id,
            MembershipPlanId = plan.Id,
            StartDate = startDate,
            EndDate = startDate.AddDays(plan.DurationInDays),
            Status = MembershipStatus.Active,
            AutoRenew = request.AutoRenew
        };

        _context.Memberships.Add(membership);
        await _context.SaveChangesAsync(cancellationToken);

        var payment = new Payment
        {
            MemberId = member.Id,
            Amount = plan.Price,
            Method = request.PaymentMethod,
            Status = PaymentStatus.Completed,
            Purpose = PaymentPurpose.Membership,
            MembershipId = membership.Id,
            TransactionReference = Guid.NewGuid().ToString("N")
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync(cancellationToken);

        return membership.Id;
    }
}
