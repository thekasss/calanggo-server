using System.Linq.Expressions;
using Calanggo.Domain.Entities;

namespace Calanggo.Domain.Interfaces.Repositories;

public interface IShortenedUrlRepository : IRepository<ShortenedUrl>
{
    Task<ShortenedUrl?> FindAsync(Expression<Func<ShortenedUrl, bool>> predicate, bool asNoTracking = false, bool includeStatistics = false);
}