using MediatR;
using TransformerMonitor.Application.DTOs;

namespace TransformerMonitor.Application.Transformers.Queries;

public record GetAllTransformersQuery() : IRequest<IEnumerable<TransformerDto>>;

public record GetTransformerByIdQuery(int Id) : IRequest<TransformerDto?>;
