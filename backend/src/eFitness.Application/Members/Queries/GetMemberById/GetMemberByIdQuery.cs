using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Members.Queries.GetMemberById;

public record GetMemberByIdQuery(int Id) : IRequest<MemberDetailDto>;

public class GetMemberByIdQueryHandler : IRequestHandler<GetMemberByIdQuery, MemberDetailDto>
{
    private readonly IApplicationDbContext _context;

    public GetMemberByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MemberDetailDto> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var member = await _context.Members
            .Include(m => m.User)
            .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (member is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Member), request.Id);
        }

        return MemberDetailDto.FromEntity(member);
    }
}
