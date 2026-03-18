using MediatR;
using Microsoft.AspNetCore.Mvc;
using TransformerMonitor.Application.DTOs;
using TransformerMonitor.Application.Transformers.Commands;
using TransformerMonitor.Application.Transformers.Queries;

namespace TransformerMonitor.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransformersController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransformersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransformerDto>>> GetAll()
    {
        return Ok(await _mediator.Send(new GetAllTransformersQuery()));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TransformerDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetTransformerByIdQuery(id));
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<TransformerDto>> Create(CreateTransformerCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateTransformerCommand command)
    {
        if (id != command.Id) return BadRequest();
        var result = await _mediator.Send(command);
        return result ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteTransformerCommand(id));
        return result ? NoContent() : NotFound();
    }
}
