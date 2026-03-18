using MediatR;
using TransformerMonitor.Application.DTOs;

namespace TransformerMonitor.Application.Tickets.Queries;

public record GetAllTicketsQuery() : IRequest<IEnumerable<TicketDto>>;

public record GetOpenTicketsQuery() : IRequest<IEnumerable<TicketDto>>;
