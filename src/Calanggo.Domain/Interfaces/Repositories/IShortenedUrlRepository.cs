using Calanggo.Domain.Entities;

namespace Calanggo.Domain.Interfaces.Repositories;

public interface IShortenedUrlRepository : IRepository<ShortenedUrl>
{
    Task<ShortenedUrl?> GetByShortCodeAsync(string shortCode, bool asNoTracking = false);
}