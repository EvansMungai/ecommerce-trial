using Ecommerce.Shared;
using Microsoft.EntityFrameworkCore;
using Order.Application.Interfaces;
using System.Linq.Expressions;

namespace Order.Infrastructure.Persistence;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private readonly OrderDbContext _context;

    public Repository(OrderDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Set<TEntity>().ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> GetSingleAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
    public async Task<TEntity?> GetSingleWithIncludeAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();
        if (includes is not null)
            query = includes.Aggregate(query, (current, include) => current.Include(include));

        return await query.FirstOrDefaultAsync(predicate, cancellationToken);
    }
    public void AddAsync(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
    }

    public void DeleteAsync(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }

    public void UpdateAsync(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
    }
}
