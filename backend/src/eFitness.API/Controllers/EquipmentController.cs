using eFitness.API.Dtos.Equipment;
using eFitness.Application.Common.Models;
using eFitness.Application.Equipment;
using eFitness.Application.Equipment.Commands.CreateEquipment;
using eFitness.Application.Equipment.Commands.DeleteEquipment;
using eFitness.Application.Equipment.Commands.UpdateEquipment;
using eFitness.Application.Equipment.Queries.GetEquipment;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eFitness.API.Controllers;

[ApiController]
[Route("api/equipment")]
[Authorize]
public class EquipmentController : ControllerBase
{
    private readonly ISender _sender;

    public EquipmentController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<EquipmentDto>>> GetEquipment(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] EquipmentCategory? category = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetEquipmentQuery(pageNumber, pageSize, searchTerm, category), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateEquipment(CreateEquipmentRequest request, CancellationToken cancellationToken)
    {
        var id = await _sender.Send(new CreateEquipmentCommand(request.Name, request.Description, request.Category, request.Quantity), cancellationToken);
        return CreatedAtAction(nameof(GetEquipment), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateEquipment(int id, UpdateEquipmentRequest request, CancellationToken cancellationToken)
    {
        await _sender.Send(new UpdateEquipmentCommand(id, request.Name, request.Description, request.Category, request.Quantity, request.IsAvailable), cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteEquipment(int id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteEquipmentCommand(id), cancellationToken);
        return NoContent();
    }
}
