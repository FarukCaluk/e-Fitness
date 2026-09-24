using eFitness.API.Dtos.Members;
using eFitness.Application.Common.Models;
using eFitness.Application.Members;
using eFitness.Application.Members.Commands.UpdateMember;
using eFitness.Application.Members.Queries.GetMemberById;
using eFitness.Application.Members.Queries.GetMembers;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eFitness.API.Controllers;

[ApiController]
[Route("api/members")]
[Authorize(Roles = "Admin")]
public class MembersController : ControllerBase
{
    private readonly ISender _sender;

    public MembersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<MemberListItemDto>>> GetMembers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] MembershipStatus? membershipStatus = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(
            new GetMembersQuery(pageNumber, pageSize, searchTerm, membershipStatus),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MemberDetailDto>> GetMember(int id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetMemberByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateMember(int id, UpdateMemberRequest request, CancellationToken cancellationToken)
    {
        await _sender.Send(
            new UpdateMemberCommand(
                id,
                request.PhoneNumber,
                request.DateOfBirth,
                request.Gender,
                request.Address,
                request.City,
                request.EmergencyContactName,
                request.EmergencyContactPhone,
                request.AssignedTrainerId),
            cancellationToken);

        return NoContent();
    }
}
