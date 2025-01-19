using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using Pingu.Core.Domain.Entities;
using Pingu.Core.Domain.Interfaces.Repositories;
using Pingu.Infrastructure.Data.Context;

namespace Pingu.Infrastructure.Data.Repositories;

public class Repository<TEntity>(PinguDbContext context) : IRepository<TEntity> where TEntity : class, IBaseEntity
{
    private readonly PinguDbContext _context = context;
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Func<TEntity, bool> predicate)
    {
        return await Task.FromResult(_dbSet.Where(predicate).ToList());
    }

    public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FirstOrDefaultAsync(entity => entity.Id == id);
    }

    public void UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
