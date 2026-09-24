using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Trainers.Queries.GetTrainerById;

public record GetTrainerByIdQuery(int Id) : IRequest<TrainerDetailDto>;

public class GetTrainerByIdQueryHandler : IRequestHandler<GetTrainerByIdQuery, TrainerDetailDto>
{
    private readonly IApplicationDbContext _context;

    public GetTrainerByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TrainerDetailDto> Handle(GetTrainerByIdQuery request, CancellationToken cancellationToken)
    {
        var trainer = await _context.Trainers
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (trainer is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Trainer), request.Id);
        }

        return TrainerDetailDto.FromEntity(trainer);
    }
}
