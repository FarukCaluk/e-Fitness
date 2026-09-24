using eFitness.API.Dtos.Orders;
using eFitness.Application.Common.Models;
using eFitness.Application.Orders;
using eFitness.Application.Orders.Commands.CreateOrder;
using eFitness.Application.Orders.Commands.UpdateOrderStatus;
using eFitness.Application.Orders.Queries.GetOrders;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eFitness.API.Controllers;

[ApiController]
[Route("api/orders")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly ISender _sender;

    public OrdersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<OrderDto>>> GetOrders(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int? memberId = null,
        [FromQuery] OrderStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetOrdersQuery(pageNumber, pageSize, memberId, status), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> CreateOrder(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var id = await _sender.Send(new CreateOrderCommand(request.Items, request.ShippingAddress, request.PaymentMethod), cancellationToken);
        return CreatedAtAction(nameof(GetOrders), new { id }, new { id });
    }

    [HttpPut("{id:int}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStatus(int id, UpdateOrderStatusRequest request, CancellationToken cancellationToken)
    {
        await _sender.Send(new UpdateOrderStatusCommand(id, request.Status), cancellationToken);
        return NoContent();
    }
}
