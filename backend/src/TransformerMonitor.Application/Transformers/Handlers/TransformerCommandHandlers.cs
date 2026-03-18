using AutoMapper;
using MediatR;
using TransformerMonitor.Application.DTOs;
using TransformerMonitor.Application.Transformers.Commands;
using TransformerMonitor.Domain.Entities;
using TransformerMonitor.Domain.Interfaces;

namespace TransformerMonitor.Application.Transformers.Handlers;

public class TransformerCommandHandlers : 
    IRequestHandler<CreateTransformerCommand, TransformerDto>,
    IRequestHandler<UpdateTransformerCommand, bool>,
    IRequestHandler<DeleteTransformerCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TransformerCommandHandlers(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TransformerDto> Handle(CreateTransformerCommand request, CancellationToken cancellationToken)
    {
        var transformer = new Transformer
        {
            AssetId = request.AssetId,
            Name = request.Name,
            Region = request.Region,
            BaseVoltage = request.BaseVoltage,
            IsActive = true
        };

        await _unitOfWork.Transformers.AddAsync(transformer);
        await _unitOfWork.CompleteAsync();

        return _mapper.Map<TransformerDto>(transformer);
    }

    public async Task<bool> Handle(UpdateTransformerCommand request, CancellationToken cancellationToken)
    {
        var transformer = await _unitOfWork.Transformers.GetByIdAsync(request.Id);
        if (transformer == null) return false;

        transformer.Name = request.Name;
        transformer.Region = request.Region;
        transformer.BaseVoltage = request.BaseVoltage;
        transformer.IsActive = request.IsActive;

        _unitOfWork.Transformers.Update(transformer);
        await _unitOfWork.CompleteAsync();
        return true;
    }

    public async Task<bool> Handle(DeleteTransformerCommand request, CancellationToken cancellationToken)
    {
        var transformer = await _unitOfWork.Transformers.GetByIdAsync(request.Id);
        if (transformer == null) return false;

        _unitOfWork.Transformers.Delete(transformer);
        await _unitOfWork.CompleteAsync();
        return true;
    }
}
