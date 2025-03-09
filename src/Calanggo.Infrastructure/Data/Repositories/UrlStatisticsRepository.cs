using System.Linq.Expressions;

using Calanggo.Application.Interfaces.Repositories;
using Calanggo.Domain.Entities;
using Calanggo.Infrastructure.Data.Context;

using Microsoft.EntityFrameworkCore;

namespace Calanggo.Infrastructure.Data.Repositories;

public class UrlStatisticsRepository(CalanggoDbContext context) : Repository<UrlStatistics>(context), IUrlStatisticsRepository
{
    public async Task<UrlStatistics?> FindAsync(Expression<Func<UrlStatistics, bool>> predicate, bool asNoTracking = false, bool include = false)
    {
        IQueryable<UrlStatistics> query = _context.Set<UrlStatistics>();

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        if (include)
        {
            query = query.Include(urlStatistics => urlStatistics.DeviceMetrics)
                .Include(urlStatistics => urlStatistics.LocationMetrics)
                .Include(urlStatistics => urlStatistics.ClickEvents);
        }

        return await query.FirstOrDefaultAsync(predicate);
    }
}