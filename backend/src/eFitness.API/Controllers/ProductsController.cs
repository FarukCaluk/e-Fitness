using eFitness.API.Dtos.Products;
using eFitness.Application.Common.Models;
using eFitness.Application.Products;
using eFitness.Application.Products.Commands.CreateProduct;
using eFitness.Application.Products.Commands.DeleteProduct;
using eFitness.Application.Products.Commands.UpdateProduct;
using eFitness.Application.Products.Queries.GetProducts;
using eFitness.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eFitness.API.Controllers;

[ApiController]
[Route("api/products")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly ISender _sender;

    public ProductsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<ProductDto>>> GetProducts(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 12,
        [FromQuery] string? searchTerm = null,
        [FromQuery] ProductCategory? category = null,
        [FromQuery] bool? isActive = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetProductsQuery(pageNumber, pageSize, searchTerm, category, isActive), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateProduct(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var id = await _sender.Send(
            new CreateProductCommand(request.Name, request.Description, request.Price, request.Category, request.StockQuantity, request.ImageUrl),
            cancellationToken);

        return CreatedAtAction(nameof(GetProducts), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateProduct(int id, UpdateProductRequest request, CancellationToken cancellationToken)
    {
        await _sender.Send(
            new UpdateProductCommand(id, request.Name, request.Description, request.Price, request.Category, request.StockQuantity, request.ImageUrl, request.IsActive),
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProduct(int id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteProductCommand(id), cancellationToken);
        return NoContent();
    }
}
