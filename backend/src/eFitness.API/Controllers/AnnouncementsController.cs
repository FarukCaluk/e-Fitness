using eFitness.API.Dtos.Announcements;
using eFitness.Application.Announcements;
using eFitness.Application.Announcements.Commands.CreateAnnouncement;
using eFitness.Application.Announcements.Queries.GetAnnouncements;
using eFitness.Application.Common.Models;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eFitness.API.Controllers;

[ApiController]
[Route("api/announcements")]
[Authorize]
public class AnnouncementsController : ControllerBase
{
    private readonly ISender _sender;

    public AnnouncementsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<AnnouncementDto>>> GetAnnouncements(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] AnnouncementSegment? segment = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetAnnouncementsQuery(pageNumber, pageSize, segment), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateAnnouncement(CreateAnnouncementRequest request, CancellationToken cancellationToken)
    {
        var id = await _sender.Send(new CreateAnnouncementCommand(request.Title, request.Body, request.Segment), cancellationToken);
        return CreatedAtAction(nameof(GetAnnouncements), new { id }, new { id });
    }
}
