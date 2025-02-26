using System.Linq.Expressions;

using Calanggo.Domain.Entities;
using Calanggo.Domain.Interfaces.Repositories;
using Calanggo.Infrastructure.Data.Context;

using Microsoft.EntityFrameworkCore;

namespace Calanggo.Infrastructure.Data.Repositories;

public class ShortenedUrlRepository(CalanggoDbContext context) : Repository<ShortenedUrl>(context), IShortenedUrlRepository {}