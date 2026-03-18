using MediatR;
using TransformerMonitor.Application.DTOs;

namespace TransformerMonitor.Application.Tickets.Queries;

public record GetAllTicketsQuery(
    string? SearchTerm = null,
    int? TeamId = null,
    string? Priority = null,
    string? Status = null,
    string? SortDir = null
) : IRequest<IEnumerable<TicketDto>>;

public record GetOpenTicketsQuery() : IRequest<IEnumerable<TicketDto>>;
