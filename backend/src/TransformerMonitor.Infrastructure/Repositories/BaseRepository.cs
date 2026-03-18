using Microsoft.EntityFrameworkCore;
using TransformerMonitor.Domain.Interfaces;
using TransformerMonitor.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace TransformerMonitor.Infrastructure.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;

    public BaseRepository(ApplicationDbContext context) => _context = context;

    public virtual async Task<T?> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);

    public virtual async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) => 
        await _context.Set<T>().Where(predicate).ToListAsync();

    public virtual async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);

    public virtual void Update(T entity) => _context.Set<T>().Update(entity);

    public virtual void Delete(T entity) => _context.Set<T>().Remove(entity);
}
