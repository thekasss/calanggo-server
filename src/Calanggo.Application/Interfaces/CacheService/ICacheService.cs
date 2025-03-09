namespace Calanggo.Application.Interfaces.CacheService;

public interface ICacheService
{
    T? Get<T>(string key);
    void Set<T>(string key, T value, TimeSpan? expirationTime = null);
    void Remove(string key);
    bool TryGet<T>(string key, out T? value);
    bool Exists(string key);
}