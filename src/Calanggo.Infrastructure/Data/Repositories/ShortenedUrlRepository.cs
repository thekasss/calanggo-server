using System.Linq.Expressions;

using Calanggo.Domain.Entities;
using Calanggo.Domain.Interfaces.Repositories;
using Calanggo.Infrastructure.Data.Context;

using Microsoft.EntityFrameworkCore;

namespace Calanggo.Infrastructure.Data.Repositories;

public class ShortenedUrlRepository(CalanggoDbContext context) : Repository<ShortenedUrl>(context), IShortenedUrlRepository
{
    public async Task<ShortenedUrl?> FindAsync(Expression<Func<ShortenedUrl, bool>> predicate, bool asNoTracking = false, bool includeStatistics = false)
    {
        var query = asNoTracking ? _dbSet.AsNoTracking() : _dbSet;
        if (includeStatistics)
        {
            query = query.Include(su => su.Statistics)
                .ThenInclude(s => s.LocationMetrics)
                .Include(su => su.Statistics)
                .ThenInclude(s => s.DeviceMetrics)
                .Include(su => su.Statistics)
                .ThenInclude(s => s.ClickEvents);
        }

        return await query.FirstOrDefaultAsync(predicate);
    }
}