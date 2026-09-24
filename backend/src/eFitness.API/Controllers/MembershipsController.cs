using eFitness.API.Dtos.Memberships;
using eFitness.Application.Common.Models;
using eFitness.Application.Memberships;
using eFitness.Application.Memberships.Commands.CancelMembership;
using eFitness.Application.Memberships.Commands.SubscribeToMembership;
using eFitness.Application.Memberships.Queries.GetMemberships;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eFitness.API.Controllers;

[ApiController]
[Route("api/memberships")]
[Authorize]
public class MembershipsController : ControllerBase
{
    private readonly ISender _sender;

    public MembershipsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<MembershipDto>>> GetMemberships(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int? memberId = null,
        [FromQuery] MembershipStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetMembershipsQuery(pageNumber, pageSize, memberId, status), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> Subscribe(SubscribeToMembershipRequest request, CancellationToken cancellationToken)
    {
        var id = await _sender.Send(new SubscribeToMembershipCommand(request.MembershipPlanId, request.PaymentMethod, request.AutoRenew), cancellationToken);
        return CreatedAtAction(nameof(GetMemberships), new { id }, new { id });
    }

    [HttpPost("{id:int}/cancel")]
    [Authorize(Roles = "Admin,Client")]
    public async Task<IActionResult> Cancel(int id, CancellationToken cancellationToken)
    {
        await _sender.Send(new CancelMembershipCommand(id), cancellationToken);
        return NoContent();
    }
}
