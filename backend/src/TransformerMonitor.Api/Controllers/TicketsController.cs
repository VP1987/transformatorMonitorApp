using MediatR;
using Microsoft.AspNetCore.Mvc;
using TransformerMonitor.Application.DTOs;
using TransformerMonitor.Application.Tickets.Commands;
using TransformerMonitor.Application.Tickets.Queries;

namespace TransformerMonitor.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TicketsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TicketDto>>> GetAll([FromQuery] GetAllTicketsQuery query)
    {
        return Ok(await _mediator.Send(query));
    }

    [HttpGet("open")]
    public async Task<ActionResult<IEnumerable<TicketDto>>> GetOpen()
    {
        return Ok(await _mediator.Send(new GetOpenTicketsQuery()));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTicketCommand command)
    {
        var result = await _mediator.Send(command);
        if (result)
        {
            return Ok();
        }
        return BadRequest();
    }

    [HttpPost("assign")]
    public async Task<IActionResult> Assign([FromBody] AssignTicketCommand command)
    {
        var result = await _mediator.Send(command);
        if (result)
        {
            return Ok();
        }
        return NotFound();
    }

    [HttpPost("{id}/resolve")]
    public async Task<IActionResult> Resolve([FromRoute] int id)
    {
        var result = await _mediator.Send(new ResolveTicketCommand(id));
        if (result)
        {
            return Ok();
        }
        return NotFound();
    }
}
