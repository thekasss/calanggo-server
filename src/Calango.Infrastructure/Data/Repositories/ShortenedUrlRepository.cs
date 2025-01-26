using Calango.Domain.Entities;
using Calango.Domain.Interfaces.Repositories;
using Calango.Infrastructure.Data.Context;

namespace Calango.Infrastructure.Data.Repositories;

public class ShortenedUrlRepository(PinguDbContext context) : Repository<ShortenedUrl>(context), IShortenedUrlRepository { }