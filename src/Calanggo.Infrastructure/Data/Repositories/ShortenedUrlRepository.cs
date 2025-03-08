using Calanggo.Domain.Entities;
using Calanggo.Domain.Interfaces.Repositories;
using Calanggo.Infrastructure.Data.Context;

using Microsoft.EntityFrameworkCore;


namespace Calanggo.Infrastructure.Data.Repositories;

public class ShortenedUrlRepository(CalanggoDbContext context) : Repository<ShortenedUrl>(context), IShortenedUrlRepository
{
    public async Task<ShortenedUrl?> GetByShortCodeAsync(string shortCode, bool asNoTracking = false)
    {
        return asNoTracking
            ? await _context.ShortenedUrls.AsNoTracking().FirstOrDefaultAsync(x => x.ShortCode == shortCode)
            : await _context.ShortenedUrls.FirstOrDefaultAsync(x => x.ShortCode == shortCode);
    }
}