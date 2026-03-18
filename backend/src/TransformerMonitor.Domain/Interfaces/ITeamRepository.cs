using TransformerMonitor.Domain.Entities;

namespace TransformerMonitor.Domain.Interfaces;

public interface ITeamRepository : IBaseRepository<Team>
{
    Task<IEnumerable<Team>> GetActiveTeamsAsync();
}
