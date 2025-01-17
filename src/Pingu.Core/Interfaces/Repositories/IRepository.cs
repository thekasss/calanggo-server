using System.Linq.Expressions;

using Pingu.Core.Domain.Entities;

namespace Pingu.Core.Interfaces.Repositories;

public interface IRepository<TEntity> where TEntity : class, IBaseEntity
{
    Task SaveChangesAsync();
    void Dispose();
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> FindAsync(Func<TEntity, bool> predicate);
    Task AddAsync(TEntity entity);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid id);
    void UpdateAsync(TEntity entity);
}