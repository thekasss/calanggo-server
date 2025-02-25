using Calanggo.Domain.Entities;
using Calanggo.Domain.Interfaces.Repositories;
using Calanggo.Infrastructure.Data.Context;

namespace Calanggo.Infrastructure.Data.Repositories;

public class UrlStatisticsRepository : Repository<UrlStatistics>, IUrlStatisticsRepository
{
    public UrlStatisticsRepository(CalanggoDbContext context) : base(context) { }
}