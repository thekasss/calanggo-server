using Pingu.Core.Domain.Entities;
using Pingu.Infrastructure.Data.Context;

namespace Pingu.Infrastructure.Data.Repositories;

public class ShortenedUrlRepository(PinguDbContext context) : Repository<ShortenedUrl>(context) { }