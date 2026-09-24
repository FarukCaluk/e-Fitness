using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using eFitness.Application.Common.Models;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.ProgressLogs.Queries.GetProgressLogs;

public record GetProgressLogsQuery(int PageNumber, int PageSize, int? MemberId) : IRequest<PaginatedList<ProgressLogDto>>;

public class GetProgressLogsQueryHandler : IRequestHandler<GetProgressLogsQuery, PaginatedList<ProgressLogDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetProgressLogsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<PaginatedList<ProgressLogDto>> Handle(GetProgressLogsQuery request, CancellationToken cancellationToken)
    {
        var memberIdFilter = request.MemberId;

        if (_currentUserService.Role == UserRole.Client)
        {
            if (_currentUserService.UserId is null)
            {
                throw new ForbiddenAccessException();
            }

            var member = await _context.Members.FirstOrDefaultAsync(m => m.UserId == _currentUserService.UserId, cancellationToken);
            memberIdFilter = member?.Id ?? -1;
        }

        var query = _context.ProgressLogs.AsQueryable();

        if (memberIdFilter is not null)
        {
            query = query.Where(p => p.MemberId == memberIdFilter);
        }

        query = query.OrderBy(p => p.RecordedAt);

        var paged = await PaginatedList<Domain.Entities.ProgressLog>.CreateAsync(query, request.PageNumber, request.PageSize);
        var items = paged.Items.Select(ProgressLogDto.FromEntity).ToList();

        return new PaginatedList<ProgressLogDto>(items, paged.TotalCount, paged.PageNumber, paged.PageSize);
    }
}
