using eFitness.Application.Orders;
using eFitness.Domain.Enums;

namespace eFitness.API.Dtos.Orders;

public record CreateOrderRequest(List<OrderItemRequestItem> Items, string? ShippingAddress, PaymentMethod PaymentMethod);

public record UpdateOrderStatusRequest(OrderStatus Status);
