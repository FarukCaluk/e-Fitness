using eFitness.API.Dtos.ProgressLogs;
using eFitness.Application.Common.Models;
using eFitness.Application.ProgressLogs;
using eFitness.Application.ProgressLogs.Commands.CreateProgressLog;
using eFitness.Application.ProgressLogs.Queries.GetProgressLogs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eFitness.API.Controllers;

[ApiController]
[Route("api/progress-logs")]
[Authorize]
public class ProgressLogsController : ControllerBase
{
    private readonly ISender _sender;

    public ProgressLogsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<ProgressLogDto>>> GetProgressLogs(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] int? memberId = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetProgressLogsQuery(pageNumber, pageSize, memberId), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> CreateProgressLog(CreateProgressLogRequest request, CancellationToken cancellationToken)
    {
        var id = await _sender.Send(
            new CreateProgressLogCommand(request.WeightKg, request.BodyFatPercentage, request.MuscleMassKg, request.Notes),
            cancellationToken);

        return CreatedAtAction(nameof(GetProgressLogs), new { id }, new { id });
    }
}
