using TransformerMonitor.Domain.Interfaces;
using TransformerMonitor.Infrastructure.Persistence;

namespace TransformerMonitor.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Transformers = new TransformerRepository(_context);
        Tickets = new TicketRepository(_context);
        Teams = new TeamRepository(_context);
    }

    public ITransformerRepository Transformers { get; private set; }
    public ITicketRepository Tickets { get; private set; }
    public ITeamRepository Teams { get; private set; }

    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}
