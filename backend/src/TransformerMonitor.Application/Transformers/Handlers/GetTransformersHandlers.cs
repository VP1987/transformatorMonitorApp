using AutoMapper;
using MediatR;
using TransformerMonitor.Application.DTOs;
using TransformerMonitor.Application.Transformers.Queries;
using TransformerMonitor.Domain.Interfaces;

namespace TransformerMonitor.Application.Transformers.Handlers;

public class GetTransformersHandlers : 
    IRequestHandler<GetAllTransformersQuery, IEnumerable<TransformerDto>>,
    IRequestHandler<GetTransformerByIdQuery, TransformerDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTransformersHandlers(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TransformerDto>> Handle(GetAllTransformersQuery request, CancellationToken cancellationToken)
    {
        var transformers = await _unitOfWork.Transformers.GetAllAsync();
        return _mapper.Map<IEnumerable<TransformerDto>>(transformers);
    }

    public async Task<TransformerDto?> Handle(GetTransformerByIdQuery request, CancellationToken cancellationToken)
    {
        var transformer = await _unitOfWork.Transformers.GetWithReadingsAsync(request.Id);
        return transformer == null ? null : _mapper.Map<TransformerDto>(transformer);
    }
}
