namespace Calanggo.Infrastructure.Configuration;

public class LocationParserOptions
{
    public required string ApiBaseUrl { get; init; }
    public required string ApiEndpoint { get; init; }
    public required string ApiFields { get; init; }
    public required TimeSpan CacheExpirationTime { get; init; }
    public required int MaxRetries { get; init; }
    public required TimeSpan RetryDelay { get; init; }
}