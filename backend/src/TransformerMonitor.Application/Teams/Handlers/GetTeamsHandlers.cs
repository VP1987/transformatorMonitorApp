using AutoMapper;
using MediatR;
using TransformerMonitor.Application.DTOs;
using TransformerMonitor.Application.Teams.Queries;
using TransformerMonitor.Domain.Interfaces;

namespace TransformerMonitor.Application.Teams.Handlers;

public class GetTeamsHandlers : 
    IRequestHandler<GetAllTeamsQuery, IEnumerable<TeamDto>>,
    IRequestHandler<GetActiveTeamsQuery, IEnumerable<TeamDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTeamsHandlers(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TeamDto>> Handle(GetAllTeamsQuery request, CancellationToken cancellationToken)
    {
        var teams = await _unitOfWork.Teams.GetAllAsync();
        return _mapper.Map<IEnumerable<TeamDto>>(teams);
    }

    public async Task<IEnumerable<TeamDto>> Handle(GetActiveTeamsQuery request, CancellationToken cancellationToken)
    {
        var teams = await _unitOfWork.Teams.GetActiveTeamsAsync();
        return _mapper.Map<IEnumerable<TeamDto>>(teams);
    }
}
