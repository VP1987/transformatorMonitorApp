using Microsoft.EntityFrameworkCore;
using TransformerMonitor.Domain.Entities;
using TransformerMonitor.Domain.Interfaces;
using TransformerMonitor.Infrastructure.Persistence;

namespace TransformerMonitor.Infrastructure.Repositories;

public class TeamRepository : BaseRepository<Team>, ITeamRepository
{
    public TeamRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Team>> GetActiveTeamsAsync()
    {
        return await _context.Teams
            .Include(x => x.Technicians)
            .Where(x => x.IsOnShiftToday)
            .ToListAsync();
    }

    public override async Task<IEnumerable<Team>> GetAllAsync()
    {
        return await _context.Teams
            .Include(x => x.Technicians)
            .ToListAsync();
    }

    public override async Task<Team?> GetByIdAsync(int id)
    {
        return await _context.Teams
            .Include(x => x.Technicians)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
