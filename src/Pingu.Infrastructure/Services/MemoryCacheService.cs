using Microsoft.Extensions.Caching.Memory;
using Pingu.Application.Interfaces;

namespace Pingu.Infrastructure.Services;

public class MemoryCacheService(IMemoryCache memoryCache) : IMemoryCacheService
{
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly MemoryCacheEntryOptions _defaultCacheOptions = new MemoryCacheEntryOptions()
        .SetSlidingExpiration(TimeSpan.FromMinutes(10))
        .SetAbsoluteExpiration(TimeSpan.FromHours(1))
        .SetPriority(CacheItemPriority.Normal)
        .SetSize(256);

    public T? Get<T>(string key)
    {
        return _memoryCache.Get<T>(key);
    }

    public bool TryGet<T>(string key, out T? value)
    {
        return _memoryCache.TryGetValue(key, out value);
    }

    public void Set<T>(string key, T value, TimeSpan? expirationTime = null)
    {
        if (expirationTime.HasValue)
        {
            _defaultCacheOptions.SetAbsoluteExpiration(expirationTime.Value);
        }

        _memoryCache.Set(key, value, _defaultCacheOptions);
    }

    public void Remove(string key)
    {
        _memoryCache.Remove(key);
    }

    public bool Exists(string key)
    {
        return _memoryCache.TryGetValue(key, out _);
    }
}