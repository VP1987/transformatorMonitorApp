namespace TransformerMonitor.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ITransformerRepository Transformers { get; }
    ITicketRepository Tickets { get; }
    ITeamRepository Teams { get; }
    Task<int> CompleteAsync();
}
