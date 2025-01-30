using Calanggo.Domain.Entities;
using Calanggo.Domain.Interfaces.Repositories;
using Calanggo.Infrastructure.Data.Context;

namespace Calanggo.Infrastructure.Data.Repositories;

public class ShortenedUrlRepository(CalangoDbContext context) : Repository<ShortenedUrl>(context), IShortenedUrlRepository { }