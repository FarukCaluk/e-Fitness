using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using eFitness.Application.Common.Models;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Orders.Queries.GetOrders;

public record GetOrdersQuery(int PageNumber, int PageSize, int? MemberId, OrderStatus? Status) : IRequest<PaginatedList<OrderDto>>;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, PaginatedList<OrderDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetOrdersQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<PaginatedList<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Orders
            .Include(o => o.Member).ThenInclude(m => m.User)
            .Include(o => o.OrderItems).ThenInclude(i => i.Product)
            .AsQueryable();

        var memberIdFilter = request.MemberId;

        if (_currentUserService.Role == UserRole.Client)
        {
            if (_currentUserService.UserId is null)
            {
                throw new ForbiddenAccessException();
            }

            var member = await _context.Members.FirstOrDefaultAsync(m => m.UserId == _currentUserService.UserId, cancellationToken);
            memberIdFilter = member?.Id ?? -1;
        }

        if (memberIdFilter is not null)
        {
            query = query.Where(o => o.MemberId == memberIdFilter);
        }

        if (request.Status is not null)
        {
            query = query.Where(o => o.Status == request.Status);
        }

        query = query.OrderByDescending(o => o.OrderDate);

        var paged = await PaginatedList<Domain.Entities.Order>.CreateAsync(query, request.PageNumber, request.PageSize);
        var items = paged.Items.Select(OrderDto.FromEntity).ToList();

        return new PaginatedList<OrderDto>(items, paged.TotalCount, paged.PageNumber, paged.PageSize);
    }
}
