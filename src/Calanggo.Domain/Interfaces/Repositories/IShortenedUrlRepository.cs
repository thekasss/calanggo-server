using Calanggo.Domain.Entities;

namespace Calanggo.Domain.Interfaces.Repositories;

public interface IShortenedUrlRepository : IRepository<ShortenedUrl>
{
    public Task<ShortenedUrl?> GetByShortCodeAsync(string shortCode, bool includeStatistics = false);
}