using MediatR;
using TransformerMonitor.Application.DTOs;

namespace TransformerMonitor.Application.Teams.Queries;

public record GetAllTeamsQuery() : IRequest<IEnumerable<TeamDto>>;

public record GetActiveTeamsQuery() : IRequest<IEnumerable<TeamDto>>;
