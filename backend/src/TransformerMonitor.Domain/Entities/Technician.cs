namespace TransformerMonitor.Domain.Entities;

public class Technician
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public int TeamId { get; set; }
    public Team? Team { get; set; }
}
