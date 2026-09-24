using eFitness.Application.Common.Interfaces;
using eFitness.Domain.Entities;
using MediatR;

namespace eFitness.Application.MembershipPlans.Commands.CreateMembershipPlan;

public record CreateMembershipPlanCommand(
    string Name,
    string? Description,
    decimal Price,
    int DurationInDays,
    bool IsFeatured,
    List<string> Features) : IRequest<int>;

public class CreateMembershipPlanCommandHandler : IRequestHandler<CreateMembershipPlanCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateMembershipPlanCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateMembershipPlanCommand request, CancellationToken cancellationToken)
    {
        var plan = new MembershipPlan
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            DurationInDays = request.DurationInDays,
            IsFeatured = request.IsFeatured,
            IsActive = true,
            Features = request.Features
                .Select((description, index) => new MembershipPlanFeature { Description = description, OrderIndex = index })
                .ToList()
        };

        _context.MembershipPlans.Add(plan);
        await _context.SaveChangesAsync(cancellationToken);

        return plan.Id;
    }
}
