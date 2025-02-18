using System.Security.Cryptography;
using System.Text;

using Calanggo.Application.Common.Results;
using Calanggo.Application.Interfaces;
using Calanggo.Application.UseCases.GetUrlStatistics;
using Calanggo.Domain.Entities;
using Calanggo.Domain.Interfaces.Repositories;

using Microsoft.Extensions.Logging;

namespace Calanggo.Infrastructure.Services;

public class UrlShortenerService(IShortenedUrlRepository shortenedUrlRepository, IMemoryCacheService memoryCacheService, ILogger<UrlShortenerService> logger)
    : IUrlShortenerService
{
    private const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const int ShortCodeLength = 7;
    private readonly ILogger<UrlShortenerService> _logger = logger;
    private readonly IMemoryCacheService _memoryCacheService = memoryCacheService;
    private readonly IShortenedUrlRepository _shortenedUrlRepository = shortenedUrlRepository;

    public async Task<Result<ShortenedUrl>> CreateShortenedUrl(string originalUrl, DateTime? expiresAt = null)
    {
        expiresAt ??= DateTime.UtcNow.AddDays(7);

        if (!Uri.TryCreate(originalUrl, UriKind.Absolute, out _))
        {
            _logger.LogError("The provided URL is not valid: {OriginalUrl}", originalUrl);
            return Result<ShortenedUrl>.Failure(new Error(400, "The provided URL is not valid."));
        }

        string shortCode = GenerateShortCode();
        ShortenedUrl shortenedUrl = new(originalUrl, shortCode, expiresAt: expiresAt);

        await _shortenedUrlRepository.AddAsync(shortenedUrl);
        await _shortenedUrlRepository.Commit();

        return Result<ShortenedUrl>.Success(shortenedUrl);
    }

    public async Task<Result<ShortenedUrl>> GetShortenedUrl(string shortCode, string ipAddress, string userAgent, string referer)
    {
        var shortenedUrl = await GetShortenedUrlFromCacheOrDatabase(shortCode);

        if (shortenedUrl is null || shortenedUrl.IsExpired())
        {
            _logger.LogError("The short code provided does not exist or has expired: {ShortCode}", shortCode);
            return Result<ShortenedUrl>.Failure(new Error(204));
        }

        await UpdateUrlStatistics(shortenedUrl, ipAddress, userAgent, referer);
        _memoryCacheService.Set(shortCode, shortenedUrl);

        return Result<ShortenedUrl>.Success(shortenedUrl);
    }

    public async Task<Result<UrlStatisticsResponse>> GetUrlStatistics(string shortCode)
    {
        var shortenedUrl = await GetShortenedUrlFromCacheOrDatabase(shortCode);

        if (shortenedUrl is null)
        {
            _logger.LogError("The short code provided does not exist: {ShortCode}", shortCode);
            return Result<UrlStatisticsResponse>.Failure(new Error(404, "URL not found"));
        }

        var statistics = shortenedUrl.Statistics;

        // TODO: Criar um método para mapear a entidade para a resposta ou utilizar um mapeador
        var response = new UrlStatisticsResponse
        {
            TotalClicks = statistics.TotalClicks,
            LastClickedAt = statistics.LastClickedAt,
            CreatedAt = statistics.CreatedAt,
            LocationMetrics = [.. statistics.LocationMetrics.Select(lm => new LocationMetricResponse
            {
                Country = lm.Country,
                Region = lm.Region,
                City = lm.City,
                Clicks = lm.Clicks
            })],
            DeviceMetrics = [.. statistics.DeviceMetrics.Select(dm => new DeviceMetricResponse
            {
                DeviceType = dm.DeviceType,
                Browser = dm.Browser,
                Clicks = dm.Clicks
            })]
        };

        return Result<UrlStatisticsResponse>.Success(response);
    }

    #region [Private Methods]

    private async Task<ShortenedUrl?> GetShortenedUrlFromCacheOrDatabase(string shortCode)
    {
        if (_memoryCacheService.TryGet(shortCode, out ShortenedUrl? shortenedUrl))
            return shortenedUrl;

        shortenedUrl ??= await _shortenedUrlRepository.FindAsync(entity => entity.ShortCode == shortCode, includeStatistics: true);
        return shortenedUrl;
    }

    private static string GenerateShortCode()
    {
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        StringBuilder result = new(ShortCodeLength);
        byte[] bytes = new byte[ShortCodeLength];

        rng.GetBytes(bytes);
        for (int i = 0; i < ShortCodeLength; i++)
        {
            result.Append(AllowedChars[bytes[i] % AllowedChars.Length]);
        }

        return result.ToString();
    }

    private async Task UpdateUrlStatistics(ShortenedUrl shortenedUrl, string ipAddress, string userAgent, string referer)
    {
        shortenedUrl.Statistics.AddClick(ipAddress, userAgent, referer);
        _shortenedUrlRepository.Update(shortenedUrl);
        await _shortenedUrlRepository.Commit();
    }

    #endregion
}