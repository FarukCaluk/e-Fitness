using eFitness.API.Dtos.TrainingSessions;
using eFitness.Application.Common.Models;
using eFitness.Application.TrainingSessions;
using eFitness.Application.TrainingSessions.Commands.CreateTrainingSession;
using eFitness.Application.TrainingSessions.Commands.UpdateTrainingSessionStatus;
using eFitness.Application.TrainingSessions.Queries.GetTrainingSessions;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eFitness.API.Controllers;

[ApiController]
[Route("api/training-sessions")]
[Authorize]
public class TrainingSessionsController : ControllerBase
{
    private readonly ISender _sender;

    public TrainingSessionsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<TrainingSessionDto>>> GetSessions(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int? trainerId = null,
        [FromQuery] int? memberId = null,
        [FromQuery] TrainingSessionStatus? status = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(
            new GetTrainingSessionsQuery(pageNumber, pageSize, trainerId, memberId, status, fromDate, toDate),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Trainer")]
    public async Task<IActionResult> CreateSession(CreateTrainingSessionRequest request, CancellationToken cancellationToken)
    {
        var id = await _sender.Send(
            new CreateTrainingSessionCommand(request.TrainerId, request.MemberId, request.ScheduledAt, request.DurationMinutes, request.Notes, request.Location),
            cancellationToken);

        return CreatedAtAction(nameof(GetSessions), new { id }, new { id });
    }

    [HttpPut("{id:int}/status")]
    [Authorize(Roles = "Admin,Trainer")]
    public async Task<IActionResult> UpdateStatus(int id, UpdateTrainingSessionStatusRequest request, CancellationToken cancellationToken)
    {
        await _sender.Send(new UpdateTrainingSessionStatusCommand(id, request.Status), cancellationToken);
        return NoContent();
    }
}
