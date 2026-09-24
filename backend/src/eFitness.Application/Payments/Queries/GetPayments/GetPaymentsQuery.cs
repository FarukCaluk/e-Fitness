using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using eFitness.Application.Common.Models;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Payments.Queries.GetPayments;

public record GetPaymentsQuery(
    int PageNumber,
    int PageSize,
    int? MemberId,
    PaymentStatus? Status,
    DateTime? FromDate,
    DateTime? ToDate) : IRequest<PaginatedList<PaymentDto>>;

public class GetPaymentsQueryHandler : IRequestHandler<GetPaymentsQuery, PaginatedList<PaymentDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetPaymentsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<PaginatedList<PaymentDto>> Handle(GetPaymentsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Payments.Include(p => p.Member).ThenInclude(m => m.User).AsQueryable();

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
            query = query.Where(p => p.MemberId == memberIdFilter);
        }

        if (request.Status is not null)
        {
            query = query.Where(p => p.Status == request.Status);
        }

        if (request.FromDate is not null)
        {
            query = query.Where(p => p.PaymentDate >= request.FromDate);
        }

        if (request.ToDate is not null)
        {
            query = query.Where(p => p.PaymentDate <= request.ToDate);
        }

        query = query.OrderByDescending(p => p.PaymentDate);

        var paged = await PaginatedList<Domain.Entities.Payment>.CreateAsync(query, request.PageNumber, request.PageSize);
        var items = paged.Items.Select(PaymentDto.FromEntity).ToList();

        return new PaginatedList<PaymentDto>(items, paged.TotalCount, paged.PageNumber, paged.PageSize);
    }
}
