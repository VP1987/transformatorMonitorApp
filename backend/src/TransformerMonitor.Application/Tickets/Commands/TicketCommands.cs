using MediatR;

namespace TransformerMonitor.Application.Tickets.Commands;

public class CreateTicketCommand : IRequest<bool>
{
    public int TransformerId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
}

public class AssignTicketCommand : IRequest<bool>
{
    public int TicketId { get; set; }
    public int TeamId { get; set; }
}

public class ResolveTicketCommand : IRequest<bool>
{
    public int TicketId { get; set; }

    public ResolveTicketCommand(int ticketId) => TicketId = ticketId;
}
