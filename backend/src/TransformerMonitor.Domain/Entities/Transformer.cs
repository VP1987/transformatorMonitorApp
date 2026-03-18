namespace TransformerMonitor.Domain.Entities;

public class Transformer
{
    public int Id { get; set; }
    public int AssetId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public double BaseVoltage { get; set; } = 22000;
    public ICollection<VoltageReading> VoltageReadings { get; set; } = new List<VoltageReading>();
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
