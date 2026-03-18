namespace TransformerMonitor.Application.DTOs;

public class TeamDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsOnShiftToday { get; set; }
    public IEnumerable<string> Technicians { get; set; } = new List<string>();
}
