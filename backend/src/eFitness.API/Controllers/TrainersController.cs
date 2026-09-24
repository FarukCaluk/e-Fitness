using eFitness.API.Dtos.Trainers;
using eFitness.Application.Common.Models;
using eFitness.Application.Trainers;
using eFitness.Application.Trainers.Commands.CreateTrainer;
using eFitness.Application.Trainers.Commands.DeleteTrainer;
using eFitness.Application.Trainers.Commands.UpdateTrainer;
using eFitness.Application.Trainers.Queries.GetMyTrainerProfile;
using eFitness.Application.Trainers.Queries.GetTrainerById;
using eFitness.Application.Trainers.Queries.GetTrainers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eFitness.API.Controllers;

[ApiController]
[Route("api/trainers")]
[Authorize]
public class TrainersController : ControllerBase
{
    private readonly ISender _sender;

    public TrainersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<TrainerListItemDto>>> GetTrainers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] bool? isAvailable = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetTrainersQuery(pageNumber, pageSize, searchTerm, isAvailable), cancellationToken);
        return Ok(result);
    }

    [HttpGet("me")]
    [Authorize(Roles = "Trainer")]
    public async Task<ActionResult<TrainerDetailDto>> GetMyProfile(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetMyTrainerProfileQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TrainerDetailDto>> GetTrainer(int id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetTrainerByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateTrainer(CreateTrainerRequest request, CancellationToken cancellationToken)
    {
        var id = await _sender.Send(
            new CreateTrainerCommand(
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName,
                request.PhoneNumber,
                request.Specialization,
                request.Bio,
                request.YearsOfExperience,
                request.HourlyRate),
            cancellationToken);

        return CreatedAtAction(nameof(GetTrainer), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateTrainer(int id, UpdateTrainerRequest request, CancellationToken cancellationToken)
    {
        await _sender.Send(
            new UpdateTrainerCommand(id, request.Specialization, request.Bio, request.YearsOfExperience, request.HourlyRate, request.IsAvailable),
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteTrainer(int id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteTrainerCommand(id), cancellationToken);
        return NoContent();
    }
}
