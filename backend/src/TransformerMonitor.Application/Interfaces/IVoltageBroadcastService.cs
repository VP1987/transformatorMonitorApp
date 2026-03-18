namespace TransformerMonitor.Application.Interfaces;

public interface IVoltageBroadcastService
{
    Task BroadcastVoltageUpdate(int transformerId, DateTime timestamp, double voltageValue);
}
