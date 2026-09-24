using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.MembershipPlans.Commands.DeleteMembershipPlan;

public record DeleteMembershipPlanCommand(int Id) : IRequest;

public class DeleteMembershipPlanCommandHandler : IRequestHandler<DeleteMembershipPlanCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteMembershipPlanCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteMembershipPlanCommand request, CancellationToken cancellationToken)
    {
        var plan = await _context.MembershipPlans.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (plan is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.MembershipPlan), request.Id);
        }

        plan.IsActive = false;
        plan.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
