using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Orders.Commands.UpdateOrderStatus;

public record UpdateOrderStatusCommand(int Id, OrderStatus Status) : IRequest;

public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateOrderStatusCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

        if (order is null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Order), request.Id);
        }

        order.Status = request.Status;
        order.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
