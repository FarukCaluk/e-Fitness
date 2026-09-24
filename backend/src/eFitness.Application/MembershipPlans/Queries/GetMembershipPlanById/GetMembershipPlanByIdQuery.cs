using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.MembershipPlans.Queries.GetMembershipPlanById;

public record GetMembershipPlanByIdQuery(int Id) : IRequest<MembershipPlanDto>;

public class GetMembershipPlanByIdQueryHandler : IRequestHandler<GetMembershipPlanByIdQuery, MembershipPlanDto>
{
    private readonly IApplicationDbContext _context;

    public GetMembershipPlanByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MembershipPlanDto> Handle(GetMembershipPlanByIdQuery request, CancellationToken cancellationToken)
    {
        var plan = await _context.MembershipPlans
            .Include(p => p.Features)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (plan is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.MembershipPlan), request.Id);
        }

        return MembershipPlanDto.FromEntity(plan);
    }
}
