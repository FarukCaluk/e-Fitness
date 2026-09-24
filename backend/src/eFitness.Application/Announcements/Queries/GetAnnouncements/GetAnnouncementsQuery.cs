using eFitness.Application.Common.Interfaces;
using eFitness.Application.Common.Models;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Announcements.Queries.GetAnnouncements;

public record GetAnnouncementsQuery(int PageNumber, int PageSize, AnnouncementSegment? Segment)
    : IRequest<PaginatedList<AnnouncementDto>>;

public class GetAnnouncementsQueryHandler : IRequestHandler<GetAnnouncementsQuery, PaginatedList<AnnouncementDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAnnouncementsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<AnnouncementDto>> Handle(GetAnnouncementsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Announcements.Include(a => a.Author).AsQueryable();

        if (request.Segment is not null)
        {
            query = query.Where(a => a.Segment == request.Segment);
        }

        query = query.OrderByDescending(a => a.CreatedAt);

        var paged = await PaginatedList<Domain.Entities.Announcement>.CreateAsync(query, request.PageNumber, request.PageSize);
        var items = paged.Items.Select(AnnouncementDto.FromEntity).ToList();

        return new PaginatedList<AnnouncementDto>(items, paged.TotalCount, paged.PageNumber, paged.PageSize);
    }
}
