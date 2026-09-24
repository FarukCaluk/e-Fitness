using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using eFitness.Application.Common.Models;
using MediatR;

namespace eFitness.Application.Notifications.Queries.GetNotifications;

public record GetNotificationsQuery(int PageNumber, int PageSize, bool? IsRead) : IRequest<PaginatedList<NotificationDto>>;

public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, PaginatedList<NotificationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetNotificationsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<PaginatedList<NotificationDto>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is null)
        {
            throw new ForbiddenAccessException();
        }

        var query = _context.Notifications.Where(n => n.UserId == _currentUserService.UserId);

        if (request.IsRead is not null)
        {
            query = query.Where(n => n.IsRead == request.IsRead);
        }

        query = query.OrderByDescending(n => n.CreatedAt);

        var paged = await PaginatedList<Domain.Entities.Notification>.CreateAsync(query, request.PageNumber, request.PageSize);
        var items = paged.Items.Select(NotificationDto.FromEntity).ToList();

        return new PaginatedList<NotificationDto>(items, paged.TotalCount, paged.PageNumber, paged.PageSize);
    }
}
