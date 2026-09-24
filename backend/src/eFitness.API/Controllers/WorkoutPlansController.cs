using eFitness.API.Dtos.WorkoutPlans;
using eFitness.Application.Common.Models;
using eFitness.Application.WorkoutPlans;
using eFitness.Application.WorkoutPlans.Commands.CreateWorkoutPlan;
using eFitness.Application.WorkoutPlans.Commands.DeleteWorkoutPlan;
using eFitness.Application.WorkoutPlans.Commands.UpdateWorkoutPlan;
using eFitness.Application.WorkoutPlans.Queries.GetWorkoutPlanById;
using eFitness.Application.WorkoutPlans.Queries.GetWorkoutPlans;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eFitness.API.Controllers;

[ApiController]
[Route("api/workout-plans")]
[Authorize]
public class WorkoutPlansController : ControllerBase
{
    private readonly ISender _sender;

    public WorkoutPlansController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<WorkoutPlanListItemDto>>> GetPlans(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int? trainerId = null,
        [FromQuery] int? memberId = null,
        [FromQuery] bool? isActive = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetWorkoutPlansQuery(pageNumber, pageSize, trainerId, memberId, isActive), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<WorkoutPlanDetailDto>> GetPlan(int id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetWorkoutPlanByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Trainer")]
    public async Task<IActionResult> CreatePlan(CreateWorkoutPlanRequest request, CancellationToken cancellationToken)
    {
        var id = await _sender.Send(
            new CreateWorkoutPlanCommand(request.TrainerId, request.MemberId, request.Title, request.Description, request.StartDate, request.EndDate, request.Exercises),
            cancellationToken);

        return CreatedAtAction(nameof(GetPlan), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,Trainer")]
    public async Task<IActionResult> UpdatePlan(int id, UpdateWorkoutPlanRequest request, CancellationToken cancellationToken)
    {
        await _sender.Send(
            new UpdateWorkoutPlanCommand(id, request.Title, request.Description, request.StartDate, request.EndDate, request.IsActive, request.Exercises),
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin,Trainer")]
    public async Task<IActionResult> DeletePlan(int id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteWorkoutPlanCommand(id), cancellationToken);
        return NoContent();
    }
}
