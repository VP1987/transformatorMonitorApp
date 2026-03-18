using AutoMapper;
using MediatR;
using TransformerMonitor.Application.DTOs;
using TransformerMonitor.Application.Tickets.Queries;
using TransformerMonitor.Application.Tickets.Commands;
using TransformerMonitor.Domain.Entities;
using TransformerMonitor.Domain.Interfaces;
using TransformerMonitor.Domain.Enums;

namespace TransformerMonitor.Application.Tickets.Handlers;

public class TicketHandlers : 
    IRequestHandler<GetAllTicketsQuery, IEnumerable<TicketDto>>,
    IRequestHandler<GetOpenTicketsQuery, IEnumerable<TicketDto>>,
    IRequestHandler<AssignTicketCommand, bool>,
    IRequestHandler<CreateTicketCommand, bool>,
    IRequestHandler<ResolveTicketCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TicketHandlers(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TicketDto>> Handle(GetAllTicketsQuery request, CancellationToken cancellationToken)
    {
        var tickets = await _unitOfWork.Tickets.GetFilteredTicketsAsync(
            request.SearchTerm, 
            request.TeamId, 
            request.Priority, 
            request.Status, 
            request.SortDir);
            
        return _mapper.Map<IEnumerable<TicketDto>>(tickets);
    }

    public async Task<IEnumerable<TicketDto>> Handle(GetOpenTicketsQuery request, CancellationToken cancellationToken)
    {
        var tickets = await _unitOfWork.Tickets.GetOpenTicketsAsync();
        return _mapper.Map<IEnumerable<TicketDto>>(tickets);
    }

    public async Task<bool> Handle(AssignTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _unitOfWork.Tickets.GetByIdAsync(request.TicketId);
        var team = await _unitOfWork.Teams.GetByIdAsync(request.TeamId);

        if (ticket == null || team == null) 
        {
            return false;
        }

        ticket.AssignedTeamId = request.TeamId;
        ticket.Status = TicketStatus.InProgress;

        _unitOfWork.Tickets.Update(ticket);
        await _unitOfWork.CompleteAsync();
        return true;
    }

    public async Task<bool> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = new Ticket
        {
            TransformerId = request.TransformerId,
            Description = request.Description,
            Priority = Enum.Parse<TicketPriority>(request.Priority),
            Status = TicketStatus.Open,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Tickets.AddAsync(ticket);
        await _unitOfWork.CompleteAsync();
        return true;
    }

    public async Task<bool> Handle(ResolveTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _unitOfWork.Tickets.GetByIdAsync(request.TicketId);
        if (ticket == null) 
        {
            return false;
        }

        ticket.Status = TicketStatus.Resolved;
        ticket.ResolvedAt = DateTime.UtcNow;

        _unitOfWork.Tickets.Update(ticket);
        await _unitOfWork.CompleteAsync();
        return true;
    }
}
