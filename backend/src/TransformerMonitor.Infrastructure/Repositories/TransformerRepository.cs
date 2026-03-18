using Microsoft.EntityFrameworkCore;
using TransformerMonitor.Domain.Entities;
using TransformerMonitor.Domain.Interfaces;
using TransformerMonitor.Infrastructure.Persistence;

namespace TransformerMonitor.Infrastructure.Repositories;

public class TransformerRepository : BaseRepository<Transformer>, ITransformerRepository
{
    public TransformerRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Transformer?> GetWithReadingsAsync(int id, int limit = 10)
    {
        return await _context.Transformers
            .Include(x => x.VoltageReadings.OrderByDescending(v => v.Timestamp).Take(limit))
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
