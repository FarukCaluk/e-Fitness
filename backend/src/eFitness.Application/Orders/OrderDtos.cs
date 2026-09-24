using eFitness.Domain.Entities;
using eFitness.Domain.Enums;

namespace eFitness.Application.Orders;

public record OrderItemDto(int ProductId, string ProductName, int Quantity, decimal UnitPrice)
{
    public static OrderItemDto FromEntity(OrderItem item) =>
        new(item.ProductId, item.Product.Name, item.Quantity, item.UnitPrice);
}

public record OrderDto(
    int Id,
    int MemberId,
    string MemberName,
    DateTime OrderDate,
    decimal TotalAmount,
    OrderStatus Status,
    string? ShippingAddress,
    List<OrderItemDto> Items)
{
    public static OrderDto FromEntity(Order order) => new(
        order.Id,
        order.MemberId,
        $"{order.Member.User.FirstName} {order.Member.User.LastName}",
        order.OrderDate,
        order.TotalAmount,
        order.Status,
        order.ShippingAddress,
        order.OrderItems.Select(OrderItemDto.FromEntity).ToList());
}

public record OrderItemRequestItem(int ProductId, int Quantity);
