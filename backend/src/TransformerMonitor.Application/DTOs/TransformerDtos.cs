namespace TransformerMonitor.Application.DTOs;

public class TransformerDto
{
    public int Id { get; set; }
    public int AssetId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public double BaseVoltage { get; set; }
    public IEnumerable<VoltageReadingDto> LastReadings { get; set; } = new List<VoltageReadingDto>();
}

public class VoltageReadingDto
{
    public DateTime Timestamp { get; set; }
    public double VoltageValue { get; set; }
}
