using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using eFitness.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.ProgressLogs.Commands.CreateProgressLog;

public record CreateProgressLogCommand(decimal WeightKg, decimal? BodyFatPercentage, decimal? MuscleMassKg, string? Notes) : IRequest<int>;

public class CreateProgressLogCommandHandler : IRequestHandler<CreateProgressLogCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateProgressLogCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(CreateProgressLogCommand request, CancellationToken cancellationToken)
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

        var log = new ProgressLog
        {
            MemberId = member.Id,
            RecordedAt = DateTime.UtcNow,
            WeightKg = request.WeightKg,
            BodyFatPercentage = request.BodyFatPercentage,
            MuscleMassKg = request.MuscleMassKg,
            Notes = request.Notes
        };

        _context.ProgressLogs.Add(log);
        await _context.SaveChangesAsync(cancellationToken);

        return log.Id;
    }
}
