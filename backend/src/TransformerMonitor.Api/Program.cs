using Microsoft.EntityFrameworkCore;
using FluentValidation;
using MediatR;
using System.Text.Json;
using TransformerMonitor.Application.Interfaces;
using TransformerMonitor.Application.Mappings;
using TransformerMonitor.Application.Transformers.Queries;
using TransformerMonitor.Application.Common.Behaviors;
using TransformerMonitor.Domain.Interfaces;
using TransformerMonitor.Infrastructure.Persistence;
using TransformerMonitor.Infrastructure.Repositories;
using TransformerMonitor.Infrastructure.Services;
using TransformerMonitor.Api.Services;
using TransformerMonitor.Api.Hubs;
using TransformerMonitor.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Better way to find the root config file
string configPath = string.Empty;
var currentDir = new DirectoryInfo(Directory.GetCurrentDirectory());

while (currentDir != null)
{
    var tempPath = Path.Combine(currentDir.FullName, "app-config.json");
    if (File.Exists(tempPath))
    {
        configPath = tempPath;
        break;
    }
    currentDir = currentDir.Parent;
}

if (string.IsNullOrEmpty(configPath))
{
    throw new FileNotFoundException("Could not find app-config.json in any parent directory.");
}

var configJson = File.ReadAllText(configPath);
using var doc = JsonDocument.Parse(configJson);

var backendPort = GetPort("BACKEND_PORT", doc.RootElement.GetProperty("Backend").GetProperty("Port").GetInt32());
var frontendPort = GetPort("FRONTEND_PORT", doc.RootElement.GetProperty("Frontend").GetProperty("Port").GetInt32());
var backendUrl = Environment.GetEnvironmentVariable("BACKEND_URL")
    ?? doc.RootElement.GetProperty("Backend").GetProperty("Url").GetString()
    ?? $"http://localhost:{backendPort}";
var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL")
    ?? doc.RootElement.GetProperty("Frontend").GetProperty("Url").GetString()
    ?? $"http://localhost:{frontendPort}";

builder.WebHost.UseUrls($"http://*:{backendPort}");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") 
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IVoltageBroadcastService, VoltageBroadcastService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddValidatorsFromAssembly(typeof(GetAllTransformersQuery).Assembly);
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(GetAllTransformersQuery).Assembly);
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

builder.Services.AddHostedService<TransformerSimulationService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DynamicCors",
        policy => policy.WithOrigins(frontendUrl!)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
});

builder.Services.AddSignalR();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("DynamicCors");
app.UseAuthorization();
app.MapControllers();
app.MapHub<TransformerHub>("/hubs/transformers");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
    await DataSeeder.SeedAsync(context);
}

app.Run();

static int GetPort(string variableName, int fallback)
{
    var value = Environment.GetEnvironmentVariable(variableName);
    return int.TryParse(value, out var port) ? port : fallback;
}
