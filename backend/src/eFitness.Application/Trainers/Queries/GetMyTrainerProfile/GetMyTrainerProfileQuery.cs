using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Trainers.Queries.GetMyTrainerProfile;

public record GetMyTrainerProfileQuery : IRequest<TrainerDetailDto>;

public class GetMyTrainerProfileQueryHandler : IRequestHandler<GetMyTrainerProfileQuery, TrainerDetailDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetMyTrainerProfileQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<TrainerDetailDto> Handle(GetMyTrainerProfileQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is null)
        {
            throw new ForbiddenAccessException();
        }

        var trainer = await _context.Trainers
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.UserId == _currentUserService.UserId, cancellationToken);

        if (trainer is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Trainer), _currentUserService.UserId.Value);
        }

        return TrainerDetailDto.FromEntity(trainer);
    }
}
