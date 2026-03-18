namespace TransformerMonitor.Domain.Entities;

public class VoltageReading
{
    public int Id { get; set; }
    public int TransformerId { get; set; }
    public DateTime Timestamp { get; set; }
    public double VoltageValue { get; set; }
    public Transformer? Transformer { get; set; }
}
