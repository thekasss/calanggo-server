using Calanggo.Domain.Entities;

namespace Calanggo.Application.Interfaces.Repositories;

public interface IShortenedUrlRepository : IRepository<ShortenedUrl>
{
    Task<ShortenedUrl?> GetByShortCodeAsync(string shortCode, bool asNoTracking = false);
}