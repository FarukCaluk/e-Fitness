using eFitness.Application.Common.Exceptions;
using eFitness.Application.Common.Interfaces;
using eFitness.Domain.Entities;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace eFitness.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand(List<OrderItemRequestItem> Items, string? ShippingAddress, PaymentMethod PaymentMethod) : IRequest<int>;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateOrderCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is null)
        {
            throw new ForbiddenAccessException();
        }

        var member = await _context.Members.FirstOrDefaultAsync(m => m.UserId == _currentUserService.UserId, cancellationToken);

        if (member is null)
        {
            throw new ForbiddenAccessException();
        }

        var productIds = request.Items.Select(i => i.ProductId).ToList();
        var products = await _context.Products
            .Where(p => productIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, cancellationToken);

        var orderItems = new List<OrderItem>();
        decimal total = 0;

        foreach (var item in request.Items)
        {
            if (!products.TryGetValue(item.ProductId, out var product))
            {
                throw new NotFoundException(nameof(Product), item.ProductId);
            }

            if (product.StockQuantity < item.Quantity)
            {
                throw new ConflictException($"Insufficient stock for \"{product.Name}\".");
            }

            product.StockQuantity -= item.Quantity;

            orderItems.Add(new OrderItem
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            });

            total += product.Price * item.Quantity;
        }

        var order = new Order
        {
            MemberId = member.Id,
            OrderDate = DateTime.UtcNow,
            TotalAmount = total,
            Status = OrderStatus.Paid,
            ShippingAddress = request.ShippingAddress,
            OrderItems = orderItems
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);

        var payment = new Payment
        {
            MemberId = member.Id,
            Amount = total,
            Method = request.PaymentMethod,
            Status = PaymentStatus.Completed,
            Purpose = PaymentPurpose.Order,
            OrderId = order.Id,
            TransactionReference = Guid.NewGuid().ToString("N")
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync(cancellationToken);

        return order.Id;
    }
}
