using eFitness.API.Dtos.MembershipPlans;
using eFitness.Application.Common.Models;
using eFitness.Application.MembershipPlans;
using eFitness.Application.MembershipPlans.Commands.CreateMembershipPlan;
using eFitness.Application.MembershipPlans.Commands.DeleteMembershipPlan;
using eFitness.Application.MembershipPlans.Commands.UpdateMembershipPlan;
using eFitness.Application.MembershipPlans.Queries.GetMembershipPlanById;
using eFitness.Application.MembershipPlans.Queries.GetMembershipPlans;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eFitness.API.Controllers;

[ApiController]
[Route("api/membership-plans")]
[Authorize]
public class MembershipPlansController : ControllerBase
{
    private readonly ISender _sender;

    public MembershipPlansController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<MembershipPlanDto>>> GetPlans(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool? isActive = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetMembershipPlansQuery(pageNumber, pageSize, isActive), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MembershipPlanDto>> GetPlan(int id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetMembershipPlanByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreatePlan(CreateMembershipPlanRequest request, CancellationToken cancellationToken)
    {
        var id = await _sender.Send(
            new CreateMembershipPlanCommand(request.Name, request.Description, request.Price, request.DurationInDays, request.IsFeatured, request.Features),
            cancellationToken);

        return CreatedAtAction(nameof(GetPlan), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdatePlan(int id, UpdateMembershipPlanRequest request, CancellationToken cancellationToken)
    {
        await _sender.Send(
            new UpdateMembershipPlanCommand(id, request.Name, request.Description, request.Price, request.DurationInDays, request.IsFeatured, request.IsActive, request.Features),
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeletePlan(int id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteMembershipPlanCommand(id), cancellationToken);
        return NoContent();
    }
}
