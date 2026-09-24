using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Members.Queries.GetMyMemberProfile;

public record GetMyMemberProfileQuery : IRequest<MemberDetailDto>;

public class GetMyMemberProfileQueryHandler : IRequestHandler<GetMyMemberProfileQuery, MemberDetailDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetMyMemberProfileQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<MemberDetailDto> Handle(GetMyMemberProfileQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is null)
        {
            throw new ForbiddenAccessException();
        }

        var member = await _context.Members
            .Include(m => m.User)
            .FirstOrDefaultAsync(m => m.UserId == _currentUserService.UserId, cancellationToken);

        if (member is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Member), _currentUserService.UserId.Value);
        }

        return MemberDetailDto.FromEntity(member);
    }
}
