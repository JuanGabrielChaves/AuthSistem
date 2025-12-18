using CleanArchitecture.Application.Commands;
using CleanArchitecture.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Recuerda enviar el Token JWT en Swagger
public class ProductsController : ControllerBase
{
    private readonly ISender _mediator; // Usamos ISender (interfaz más específica de MediatR)

    public ProductsController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(id));

        // Aplicamos el Result Pattern también aquí
        if (!result.IsSuccess)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        // result.Value contiene el ID del nuevo producto
        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Value },
            result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        // Corregido: usamos _mediator en lugar de _sender
        var result = await _mediator.Send(new GetAllProductsQuery());

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Error);
    }
}