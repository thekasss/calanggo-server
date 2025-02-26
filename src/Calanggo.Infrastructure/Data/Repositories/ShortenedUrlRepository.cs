
using Calanggo.Domain.Entities;
using Calanggo.Domain.Interfaces.Repositories;
using Calanggo.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Calanggo.Infrastructure.Data.Repositories;

public class ShortenedUrlRepository(CalanggoDbContext context) : Repository<ShortenedUrl>(context), IShortenedUrlRepository
{
    public async Task<ShortenedUrl?> GetByShortCodeAsync(string shortCode, bool includeStatistics = false)
    {
        var query = _context.ShortenedUrls.AsQueryable();

        if (includeStatistics)
        {
            query = query
                .Include(s => s.Statistics)
                .ThenInclude(s => s.ClickEvents)
                .Include(s => s.Statistics)
                .ThenInclude(s => s.DeviceMetrics)
                .Include(s => s.Statistics)
                .ThenInclude(s => s.LocationMetrics);
        }

        return await query.FirstOrDefaultAsync(s => s.ShortCode == shortCode);
    }
}