using Catalog.Application.Interfaces;
using Ecommerce.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Catalog.Infrastructure.Persistence;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private readonly CatalogDbContext _context;

    public Repository(CatalogDbContext context)
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

    public async Task<IEnumerable<TEntity?>> GetFilteredAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _context.Set<TEntity>().Where(predicate).ToListAsync(cancellationToken);
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
