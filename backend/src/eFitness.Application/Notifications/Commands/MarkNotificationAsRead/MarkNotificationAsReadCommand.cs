using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Notifications.Commands.MarkNotificationAsRead;

public record MarkNotificationAsReadCommand(int Id) : IRequest;

public class MarkNotificationAsReadCommandHandler : IRequestHandler<MarkNotificationAsReadCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public MarkNotificationAsReadCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
    {
        var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.Id == request.Id, cancellationToken);

        if (notification is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Notification), request.Id);
        }

        if (notification.UserId != _currentUserService.UserId)
        {
            throw new ForbiddenAccessException();
        }

        notification.IsRead = true;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
