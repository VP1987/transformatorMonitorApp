using Microsoft.EntityFrameworkCore;
using TransformerMonitor.Domain.Entities;
using TransformerMonitor.Domain.Interfaces;
using TransformerMonitor.Infrastructure.Persistence;
using TransformerMonitor.Domain.Enums;

namespace TransformerMonitor.Infrastructure.Repositories;

public class TicketRepository : BaseRepository<Ticket>, ITicketRepository
{
    public TicketRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Ticket>> GetFilteredTicketsAsync(string? searchTerm, int? teamId, string? priorityStr, string? statusStr, string? sortDir)
    {
        var query = _context.Tickets
            .Include(x => x.Transformer)
            .Include(x => x.AssignedTeam)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var lowerTerm = searchTerm.ToLower();
            query = query.Where(x => 
                (x.Transformer != null && x.Transformer.Name.ToLower().Contains(lowerTerm)) || 
                (x.Description != null && x.Description.ToLower().Contains(lowerTerm)));
        }

        if (teamId.HasValue)
        {
            query = query.Where(x => x.AssignedTeamId == teamId.Value);
        }

        if (!string.IsNullOrWhiteSpace(priorityStr) && Enum.TryParse<TicketPriority>(priorityStr, true, out var priority))
        {
            query = query.Where(x => x.Priority == priority);
        }

        if (!string.IsNullOrWhiteSpace(statusStr))
        {
            if (statusStr.Equals("Active", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(x => x.Status != TicketStatus.Resolved);
            }
            else if (Enum.TryParse<TicketStatus>(statusStr, true, out var status))
            {
                query = query.Where(x => x.Status == status);
            }
        }

        if (string.Equals(sortDir, "asc", StringComparison.OrdinalIgnoreCase))
        {
            query = query.OrderBy(x => x.CreatedAt);
        }
        else
        {
            query = query.OrderByDescending(x => x.CreatedAt);
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Ticket>> GetOpenTicketsAsync()
    {
        return await _context.Tickets
            .Include(x => x.Transformer)
            .Include(x => x.AssignedTeam)
            .Where(x => x.Status != TicketStatus.Resolved)
            .ToListAsync();
    }

    public override async Task<IEnumerable<Ticket>> GetAllAsync()
    {
        return await _context.Tickets
            .Include(x => x.Transformer)
            .Include(x => x.AssignedTeam)
            .ToListAsync();
    }

    public override async Task<Ticket?> GetByIdAsync(int id)
    {
        return await _context.Tickets
            .Include(x => x.Transformer)
            .Include(x => x.AssignedTeam)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
