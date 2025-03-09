using System.Net.Http.Json;
using System.Text.Json;

using Calanggo.Domain.Interfaces;
using Calanggo.Infrastructure.Configuration;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Calanggo.Infrastructure.Services;

public class LocationParserService(HttpClient httpClient, IMemoryCache cache, ILogger<LocationParserService> logger, IOptions<LocationParserOptions> options)
    : ILocationParserService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IMemoryCache _cache = cache;
    private readonly ILogger<LocationParserService> _logger = logger;
    private readonly LocationParserOptions _options = options.Value;

    public (string Country, string Region, string City) Parse(string ipAddress)
        => ParseAsync(ipAddress).GetAwaiter().GetResult();

    public async Task<(string Country, string Region, string City)> ParseAsync(string ipAddress, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(ipAddress) || ipAddress == "127.0.0.1" || ipAddress == "::1")
            return ("Local", "Local", "Local");


        // Verify if we have this IP in cache
        string cacheKey = $"IP_GEO_{ipAddress}";
        if (_cache.TryGetValue(cacheKey, out (string Country, string Region, string City) cachedLocation))
        {
            _logger.LogDebug("IP location found in cache for {IpAddress}", ipAddress);
            return cachedLocation;
        }

        try
        {
            return await GetLocationWithRetryAsync(ipAddress, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting location for IP address: {IpAddress}", ipAddress);
            return new("Unknown", "Unknown", "Unknown");
        }
    }

    # region [Private Methods]
    private async Task<(string Country, string Region, string City)> GetLocationWithRetryAsync(string ipAddress, CancellationToken cancellationToken)
    {
        int attempt = 0;
        Exception lastException = null!;

        while (attempt < _options.MaxRetries)
        {
            try
            {
                var location = await FetchLocationAsync(ipAddress, cancellationToken);

                // cache the result
                string cacheKey = $"IP_GEO_{ipAddress}";
                _cache.Set(cacheKey, location, _options.CacheExpirationTime);

                return location;
            }
            catch (Exception ex)
            {
                lastException = ex;
                attempt++;

                if (attempt < _options.MaxRetries)
                {
                    _logger.LogWarning(ex, "Retry {Attempt} of {MaxRetries} for IP: {IpAddress}",
                        attempt, _options.MaxRetries, ipAddress);
                    await Task.Delay(_options.RetryDelay, cancellationToken);
                }
            }
        }

        throw new Exception($"Failed to get location after {_options.MaxRetries} attempts", lastException);
    }

    private record GeoIpResponse(string Country, string RegionName, string City);
    private async Task<(string Country, string Region, string City)> FetchLocationAsync(string ipAddress, CancellationToken cancellationToken)
    {
        string url = $"{_options.ApiBaseUrl}/{_options.ApiEndpoint}/{ipAddress}?fields={_options.ApiFields}";
        var response = await _httpClient.GetFromJsonAsync<GeoIpResponse>(url, cancellationToken)
            ?? throw new JsonException("Failed to deserialize geolocation response");

        return new(
            string.IsNullOrEmpty(response.Country) ? "Unknown" : response.Country,
            string.IsNullOrEmpty(response.RegionName) ? "Unknown" : response.RegionName,
            string.IsNullOrEmpty(response.City) ? "Unknown" : response.City
        );
    }

    # endregion
}