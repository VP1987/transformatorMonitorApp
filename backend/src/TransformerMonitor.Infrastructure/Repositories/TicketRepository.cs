using Microsoft.EntityFrameworkCore;
using TransformerMonitor.Domain.Entities;
using TransformerMonitor.Domain.Interfaces;
using TransformerMonitor.Infrastructure.Persistence;

namespace TransformerMonitor.Infrastructure.Repositories;

public class TicketRepository : BaseRepository<Ticket>, ITicketRepository
{
    public TicketRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Ticket>> GetOpenTicketsAsync()
    {
        return await _context.Tickets
            .Include(x => x.Transformer)
            .Include(x => x.AssignedTeam)
            .Where(x => x.Status != TransformerMonitor.Domain.Enums.TicketStatus.Resolved)
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
