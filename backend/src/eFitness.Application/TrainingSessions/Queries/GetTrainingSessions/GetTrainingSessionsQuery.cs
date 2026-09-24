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

    public GetTrainingSessionsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<TrainingSessionDto>> Handle(GetTrainingSessionsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.TrainingSessions
            .Include(s => s.Trainer).ThenInclude(t => t.User)
            .Include(s => s.Member).ThenInclude(m => m.User)
            .AsQueryable();

        if (request.TrainerId is not null)
        {
            query = query.Where(s => s.TrainerId == request.TrainerId);
        }

        if (request.MemberId is not null)
        {
            query = query.Where(s => s.MemberId == request.MemberId);
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
