using TransformerMonitor.Domain.Entities;

namespace TransformerMonitor.Domain.Interfaces;

public interface ITransformerRepository : IBaseRepository<Transformer>
{
    Task<Transformer?> GetWithReadingsAsync(int id, int limit = 10);
}
