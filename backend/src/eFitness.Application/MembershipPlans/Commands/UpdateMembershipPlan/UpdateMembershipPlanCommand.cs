using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using eFitness.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.MembershipPlans.Commands.UpdateMembershipPlan;

public record UpdateMembershipPlanCommand(
    int Id,
    string Name,
    string? Description,
    decimal Price,
    int DurationInDays,
    bool IsFeatured,
    bool IsActive,
    List<string> Features) : IRequest;

public class UpdateMembershipPlanCommandHandler : IRequestHandler<UpdateMembershipPlanCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateMembershipPlanCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateMembershipPlanCommand request, CancellationToken cancellationToken)
    {
        var plan = await _context.MembershipPlans
            .Include(p => p.Features)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (plan is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.MembershipPlan), request.Id);
        }

        plan.Name = request.Name;
        plan.Description = request.Description;
        plan.Price = request.Price;
        plan.DurationInDays = request.DurationInDays;
        plan.IsFeatured = request.IsFeatured;
        plan.IsActive = request.IsActive;
        plan.UpdatedAt = DateTime.UtcNow;

        plan.Features.Clear();
        for (var index = 0; index < request.Features.Count; index++)
        {
            plan.Features.Add(new MembershipPlanFeature { Description = request.Features[index], OrderIndex = index });
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
