namespace TransformerMonitor.Application.DTOs;

public class TicketDto
{
    public int Id { get; set; }
    public int TransformerId { get; set; }
    public string TransformerName { get; set; } = string.Empty;
    public int? AssignedTeamId { get; set; }
    public string? AssignedTeamName { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
