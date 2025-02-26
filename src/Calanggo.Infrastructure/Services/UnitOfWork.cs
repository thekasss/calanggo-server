using Calanggo.Application.Interfaces;
using Calanggo.Domain.Interfaces.Repositories;
using Calanggo.Infrastructure.Data.Context;
using Calanggo.Infrastructure.Data.Repositories;

namespace Calanggo.Infrastructure.Services;

public class UnitOfWork : IDisposable
{
    public CalanggoDbContext DbContext { get; init; }
    public IUrlStatisticsRepository UrlStatisticsRepository { get; init; }
    public IShortenedUrlRepository ShortenedUrlRepository { get; init; }
    private bool _disposed;

    public UnitOfWork(IUrlStatisticsRepository urlStatisticsRepository, IShortenedUrlRepository shortenedUrlRepository,
        CalanggoDbContext dbContext)
    {
        DbContext = dbContext;
        UrlStatisticsRepository = urlStatisticsRepository;
        ShortenedUrlRepository = shortenedUrlRepository;
    }

    public async Task Commit(CancellationToken cancellationToken = default)
    {
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                DbContext?.Dispose();
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}