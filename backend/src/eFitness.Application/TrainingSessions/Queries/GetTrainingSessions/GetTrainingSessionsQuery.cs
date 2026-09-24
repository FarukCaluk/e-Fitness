using eFitness.Application.Common.Interfaces;
using eFitness.Application.Common.Models;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.TrainingSessions.Queries.GetTrainingSessions;

public record GetTrainingSessionsQuery(
    int PageNumber,
    int PageSize,
    int? TrainerId,
    int? MemberId,
    TrainingSessionStatus? Status,
    DateTime? FromDate,
    DateTime? ToDate) : IRequest<PaginatedList<TrainingSessionDto>>;

public class GetTrainingSessionsQueryHandler : IRequestHandler<GetTrainingSessionsQuery, PaginatedList<TrainingSessionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetTrainingSessionsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<PaginatedList<TrainingSessionDto>> Handle(GetTrainingSessionsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.TrainingSessions
            .Include(s => s.Trainer).ThenInclude(t => t.User)
            .Include(s => s.Member).ThenInclude(m => m.User)
            .AsQueryable();

        var trainerIdFilter = request.TrainerId;
        var memberIdFilter = request.MemberId;

        if (_currentUserService.Role == UserRole.Trainer)
        {
            var trainer = await _context.Trainers.FirstOrDefaultAsync(t => t.UserId == _currentUserService.UserId, cancellationToken);
            trainerIdFilter = trainer?.Id ?? -1;
        }
        else if (_currentUserService.Role == UserRole.Client)
        {
            var member = await _context.Members.FirstOrDefaultAsync(m => m.UserId == _currentUserService.UserId, cancellationToken);
            memberIdFilter = member?.Id ?? -1;
        }

        if (trainerIdFilter is not null)
        {
            query = query.Where(s => s.TrainerId == trainerIdFilter);
        }

        if (memberIdFilter is not null)
        {
            query = query.Where(s => s.MemberId == memberIdFilter);
        }

        if (request.Status is not null)
        {
            query = query.Where(s => s.Status == request.Status);
        }

        if (request.FromDate is not null)
        {
            query = query.Where(s => s.ScheduledAt >= request.FromDate);
        }

        if (request.ToDate is not null)
        {
            query = query.Where(s => s.ScheduledAt <= request.ToDate);
        }

        query = query.OrderBy(s => s.ScheduledAt);

        var paged = await PaginatedList<Domain.Entities.TrainingSession>.CreateAsync(query, request.PageNumber, request.PageSize);
        var items = paged.Items.Select(TrainingSessionDto.FromEntity).ToList();

        return new PaginatedList<TrainingSessionDto>(items, paged.TotalCount, paged.PageNumber, paged.PageSize);
    }
}
