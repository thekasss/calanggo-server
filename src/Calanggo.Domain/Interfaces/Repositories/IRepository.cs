using System.Linq.Expressions;

using Calanggo.Domain.Entities;

namespace Calanggo.Domain.Interfaces.Repositories;

public interface IRepository<TEntity> where TEntity : class, IBaseEntity
{
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = false);
    Task<IEnumerable<TEntity>> FindListAsync(Func<TEntity, bool> predicate, bool asNoTracking = false);
    Task AddAsync(TEntity entity);
    Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false);
    Task<TEntity?> GetByIdAsync(Guid id, bool asNoTracking = false);
    Task DeleteAsync(Guid id);
    void Update(TEntity entity);
}