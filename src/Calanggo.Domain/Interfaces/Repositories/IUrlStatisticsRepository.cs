using System.Linq.Expressions;
using Calanggo.Domain.Entities;

namespace Calanggo.Domain.Interfaces.Repositories;

public interface IUrlStatisticsRepository : IRepository<UrlStatistics>
{
    Task<UrlStatistics?> FindAsync(Expression<Func<UrlStatistics, bool>> predicate, bool asNoTracking = false, bool include = false);
}