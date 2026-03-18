using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TransformerMonitor.Application.Interfaces;
using TransformerMonitor.Domain.Entities;
using TransformerMonitor.Infrastructure.Persistence;

namespace TransformerMonitor.Infrastructure.Services;

public class TransformerSimulationService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TransformerSimulationService> _logger;
    private readonly Random _random = new();

    public TransformerSimulationService(IServiceProvider serviceProvider, ILogger<TransformerSimulationService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var broadcastService = scope.ServiceProvider.GetRequiredService<IVoltageBroadcastService>();
                var transformers = context.Transformers.Where(t => t.IsActive).ToList();

                foreach (var transformer in transformers)
                {
                    var fluctuation = (_random.NextDouble() - 0.5) * 400;
                    var reading = new VoltageReading
                    {
                        TransformerId = transformer.Id,
                        Timestamp = DateTime.UtcNow,
                        VoltageValue = Math.Round(transformer.BaseVoltage + fluctuation, 2)
                    };

                    context.VoltageReadings.Add(reading);

                    await broadcastService.BroadcastVoltageUpdate(transformer.Id, reading.Timestamp, reading.VoltageValue);
                }

                await context.SaveChangesAsync(stoppingToken);
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}
