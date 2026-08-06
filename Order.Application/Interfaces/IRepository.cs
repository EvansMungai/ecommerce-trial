using Ecommerce.Shared;
using System.Linq.Expressions;

namespace Order.Application.Interfaces;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<TEntity?> GetSingleAsync(int id, CancellationToken cancellationToken);
    Task<TEntity?> GetSingleWithIncludeAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includes);
    void AddAsync(TEntity entity);
    void UpdateAsync(TEntity entity);
    void DeleteAsync(TEntity entity);
}
