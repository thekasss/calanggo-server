using Pingu.Core.Domain.Entities;
using Pingu.Infrastructure.Data.Context;

namespace Pingu.Infrastructure.Data.Repositories;

public class UrlStatisticsRepository(PinguDbContext context) : Repository<UrlStatistics>(context) { }