using MediatR;
using Microsoft.AspNetCore.Mvc;
using TransformerMonitor.Application.DTOs;
using TransformerMonitor.Application.Teams.Queries;

namespace TransformerMonitor.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TeamsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeamDto>>> GetAll()
    {
        return Ok(await _mediator.Send(new GetAllTeamsQuery()));
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<TeamDto>>> GetActive()
    {
        return Ok(await _mediator.Send(new GetActiveTeamsQuery()));
    }
}
