using Microsoft.AspNetCore.SignalR;
using TransformerMonitor.Application.DTOs;

namespace TransformerMonitor.Api.Hubs;

public class TransformerHub : Hub
{
    public async Task SubscribeToTransformer(int transformerId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Transformer_{transformerId}");
    }

    public async Task UnsubscribeFromTransformer(int transformerId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Transformer_{transformerId}");
    }
}
