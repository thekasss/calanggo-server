using System.Linq.Expressions;
using Calango.Domain.Entities;

namespace Calango.Domain.Interfaces.Repositories;

public interface IRepository<TEntity> where TEntity : class, IBaseEntity
{
    Task Commit();
    void Dispose();
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = false);
    Task<IEnumerable<TEntity>> FindListAsync(Func<TEntity, bool> predicate, bool asNoTracking = false);
    Task AddAsync(TEntity entity);
    Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false);
    Task<TEntity?> GetByIdAsync(Guid id, bool asNoTracking = false);
    Task DeleteAsync(Guid id);
    void UpdateAsync(TEntity entity);
}