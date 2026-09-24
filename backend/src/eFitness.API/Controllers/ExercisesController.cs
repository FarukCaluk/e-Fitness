using eFitness.API.Dtos.Exercises;
using eFitness.Application.Common.Models;
using eFitness.Application.Exercises;
using eFitness.Application.Exercises.Commands.CreateExercise;
using eFitness.Application.Exercises.Commands.DeleteExercise;
using eFitness.Application.Exercises.Commands.UpdateExercise;
using eFitness.Application.Exercises.Queries.GetExercises;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eFitness.API.Controllers;

[ApiController]
[Route("api/exercises")]
[Authorize]
public class ExercisesController : ControllerBase
{
    private readonly ISender _sender;

    public ExercisesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<ExerciseDto>>> GetExercises(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] MuscleGroup? muscleGroup = null,
        [FromQuery] ExerciseDifficulty? difficulty = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetExercisesQuery(pageNumber, pageSize, searchTerm, muscleGroup, difficulty), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Trainer")]
    public async Task<IActionResult> CreateExercise(CreateExerciseRequest request, CancellationToken cancellationToken)
    {
        var id = await _sender.Send(
            new CreateExerciseCommand(request.Name, request.Description, request.MuscleGroup, request.Difficulty, request.VideoUrl, request.EquipmentId),
            cancellationToken);

        return CreatedAtAction(nameof(GetExercises), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,Trainer")]
    public async Task<IActionResult> UpdateExercise(int id, UpdateExerciseRequest request, CancellationToken cancellationToken)
    {
        await _sender.Send(
            new UpdateExerciseCommand(id, request.Name, request.Description, request.MuscleGroup, request.Difficulty, request.VideoUrl, request.EquipmentId),
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteExercise(int id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteExerciseCommand(id), cancellationToken);
        return NoContent();
    }
}
