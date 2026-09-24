using eFitness.API.Dtos.Members;
using eFitness.Application.Common.Models;
using eFitness.Application.Members;
using eFitness.Application.Members.Commands.UpdateMember;
using eFitness.Application.Members.Queries.GetMemberById;
using eFitness.Application.Members.Queries.GetMembers;
using eFitness.Application.Members.Queries.GetMyClients;
using eFitness.Application.Members.Queries.GetMyMemberProfile;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eFitness.API.Controllers;

[ApiController]
[Route("api/members")]
[Authorize]
public class MembersController : ControllerBase
{
    private readonly ISender _sender;

    public MembersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
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

    [HttpGet("me")]
    [Authorize(Roles = "Client")]
    public async Task<ActionResult<MemberDetailDto>> GetMyProfile(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetMyMemberProfileQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("my-clients")]
    [Authorize(Roles = "Trainer")]
    public async Task<ActionResult<PaginatedList<MemberListItemDto>>> GetMyClients(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetMyClientsQuery(pageNumber, pageSize, searchTerm), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<MemberDetailDto>> GetMember(int id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetMemberByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
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
