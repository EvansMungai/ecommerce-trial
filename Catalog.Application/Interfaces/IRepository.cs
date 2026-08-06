using Ecommerce.Shared;
using System.Linq.Expressions;

namespace Catalog.Application.Interfaces;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<TEntity?> GetSingleAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<TEntity?>> GetFilteredAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    void AddAsync(TEntity entity);
    void UpdateAsync(TEntity entity);
    void DeleteAsync(TEntity entity);
}
