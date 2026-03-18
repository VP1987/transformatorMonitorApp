using TransformerMonitor.Domain.Entities;

namespace TransformerMonitor.Domain.Interfaces;

public interface ITicketRepository : IBaseRepository<Ticket>
{
    Task<IEnumerable<Ticket>> GetOpenTicketsAsync();
    Task<IEnumerable<Ticket>> GetFilteredTicketsAsync(string? searchTerm, int? teamId, string? priority, string? status, string? sortDir);
}
