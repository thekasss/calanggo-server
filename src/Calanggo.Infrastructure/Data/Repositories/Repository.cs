using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Calanggo.Domain.Entities;
using Calanggo.Domain.Interfaces.Repositories;
using Calanggo.Infrastructure.Data.Context;

namespace Calanggo.Infrastructure.Data.Repositories;

public class Repository<TEntity>(CalanggoDbContext context) : IRepository<TEntity> where TEntity : class, IBaseEntity
{
    protected readonly CalanggoDbContext _context = context;
    protected readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        TEntity? entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }

    public async Task<IEnumerable<TEntity>> FindListAsync(Func<TEntity, bool> predicate, bool asNoTracking = false)
    {
        return asNoTracking
            ? await Task.FromResult(_dbSet.AsNoTracking().Where(predicate).ToList())
            : (IEnumerable<TEntity>)await Task.FromResult(_dbSet.Where(predicate).ToList());
    }

    public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = false)
    {
        return asNoTracking
            ? await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate)
            : await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false)
    {
        return asNoTracking
            ? await _dbSet.AsNoTracking().ToListAsync()
            : await _dbSet.ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, bool asNoTracking = false)
    {
        return asNoTracking
            ? await _dbSet.AsNoTracking().FirstOrDefaultAsync(entity => entity.Id == id)
            : await _dbSet.FirstOrDefaultAsync(entity => entity.Id == id);
    }

    public void Update(TEntity entity)
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