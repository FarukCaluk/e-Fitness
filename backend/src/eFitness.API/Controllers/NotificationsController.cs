using eFitness.Application.Common.Models;
using eFitness.Application.Notifications;
using eFitness.Application.Notifications.Commands.MarkNotificationAsRead;
using eFitness.Application.Notifications.Queries.GetNotifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eFitness.API.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly ISender _sender;

    public NotificationsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<NotificationDto>>> GetNotifications(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool? isRead = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetNotificationsQuery(pageNumber, pageSize, isRead), cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:int}/read")]
    public async Task<IActionResult> MarkAsRead(int id, CancellationToken cancellationToken)
    {
        await _sender.Send(new MarkNotificationAsReadCommand(id), cancellationToken);
        return NoContent();
    }
}
