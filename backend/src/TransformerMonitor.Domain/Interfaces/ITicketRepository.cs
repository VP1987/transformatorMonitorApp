using TransformerMonitor.Domain.Entities;

namespace TransformerMonitor.Domain.Interfaces;

public interface ITicketRepository : IBaseRepository<Ticket>
{
    Task<IEnumerable<Ticket>> GetOpenTicketsAsync();
}
