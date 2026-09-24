using eFitness.Application.Common.Interfaces;
using eFitness.Application.Common.Models;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Exercises.Queries.GetExercises;

public record GetExercisesQuery(
    int PageNumber,
    int PageSize,
    string? SearchTerm,
    MuscleGroup? MuscleGroup,
    ExerciseDifficulty? Difficulty) : IRequest<PaginatedList<ExerciseDto>>;

public class GetExercisesQueryHandler : IRequestHandler<GetExercisesQuery, PaginatedList<ExerciseDto>>
{
    private readonly IApplicationDbContext _context;

    public GetExercisesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<ExerciseDto>> Handle(GetExercisesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Exercises.Include(e => e.Equipment).AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim().ToLower();
            query = query.Where(e => e.Name.ToLower().Contains(term));
        }

        if (request.MuscleGroup is not null)
        {
            query = query.Where(e => e.MuscleGroup == request.MuscleGroup);
        }

        if (request.Difficulty is not null)
        {
            query = query.Where(e => e.Difficulty == request.Difficulty);
        }

        query = query.OrderBy(e => e.Name);

        var paged = await PaginatedList<Domain.Entities.Exercise>.CreateAsync(query, request.PageNumber, request.PageSize);
        var items = paged.Items.Select(ExerciseDto.FromEntity).ToList();

        return new PaginatedList<ExerciseDto>(items, paged.TotalCount, paged.PageNumber, paged.PageSize);
    }
}
