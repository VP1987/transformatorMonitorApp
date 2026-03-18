using TransformerMonitor.Domain.Enums;

namespace TransformerMonitor.Domain.Entities;

public class Ticket
{
    public int Id { get; set; }
    public int TransformerId { get; set; }
    public int? AssignedTeamId { get; set; }
    public TicketStatus Status { get; set; }
    public TicketPriority Priority { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public Transformer? Transformer { get; set; }
    public Team? AssignedTeam { get; set; }
}
