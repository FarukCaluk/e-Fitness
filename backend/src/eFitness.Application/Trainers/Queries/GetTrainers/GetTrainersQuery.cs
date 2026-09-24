using eFitness.Application.Common.Interfaces;
using eFitness.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Trainers.Queries.GetTrainers;

public record GetTrainersQuery(
    int PageNumber,
    int PageSize,
    string? SearchTerm,
    bool? IsAvailable) : IRequest<PaginatedList<TrainerListItemDto>>;

public class GetTrainersQueryHandler : IRequestHandler<GetTrainersQuery, PaginatedList<TrainerListItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetTrainersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<TrainerListItemDto>> Handle(GetTrainersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Trainers.Include(t => t.User).AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim().ToLower();
            query = query.Where(t =>
                t.User.FirstName.ToLower().Contains(term) ||
                t.User.LastName.ToLower().Contains(term) ||
                t.Specialization.ToLower().Contains(term));
        }

        if (request.IsAvailable is not null)
        {
            query = query.Where(t => t.IsAvailable == request.IsAvailable);
        }

        query = query.OrderByDescending(t => t.Rating);

        var paged = await PaginatedList<Domain.Entities.Trainer>.CreateAsync(query, request.PageNumber, request.PageSize);
        var items = paged.Items.Select(TrainerListItemDto.FromEntity).ToList();

        return new PaginatedList<TrainerListItemDto>(items, paged.TotalCount, paged.PageNumber, paged.PageSize);
    }
}
