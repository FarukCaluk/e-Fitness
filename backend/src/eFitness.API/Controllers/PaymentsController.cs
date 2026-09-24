using eFitness.Application.Common.Models;
using eFitness.Application.Payments;
using eFitness.Application.Payments.Queries.GetPayments;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eFitness.API.Controllers;

[ApiController]
[Route("api/payments")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly ISender _sender;

    public PaymentsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<PaymentDto>>> GetPayments(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int? memberId = null,
        [FromQuery] PaymentStatus? status = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetPaymentsQuery(pageNumber, pageSize, memberId, status, fromDate, toDate), cancellationToken);
        return Ok(result);
    }
}
