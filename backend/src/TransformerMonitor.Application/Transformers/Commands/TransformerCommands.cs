using MediatR;
using TransformerMonitor.Application.DTOs;

namespace TransformerMonitor.Application.Transformers.Commands;

public record CreateTransformerCommand(int AssetId, string Name, string Region, double BaseVoltage) : IRequest<TransformerDto>;

public record UpdateTransformerCommand(int Id, string Name, string Region, double BaseVoltage, bool IsActive) : IRequest<bool>;

public record DeleteTransformerCommand(int Id) : IRequest<bool>;
