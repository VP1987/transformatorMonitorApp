using Microsoft.AspNetCore.SignalR;
using TransformerMonitor.Api.Hubs;
using TransformerMonitor.Application.Interfaces;

namespace TransformerMonitor.Api.Services;

public class VoltageBroadcastService : IVoltageBroadcastService
{
    private readonly IHubContext<TransformerHub> _hubContext;

    public VoltageBroadcastService(IHubContext<TransformerHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task BroadcastVoltageUpdate(int transformerId, DateTime timestamp, double voltageValue)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveVoltageUpdate", new
        {
            TransformerId = transformerId,
            Timestamp = timestamp,
            VoltageValue = voltageValue
        });
    }
}
