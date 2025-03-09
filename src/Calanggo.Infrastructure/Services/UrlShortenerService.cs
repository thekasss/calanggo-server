using System.Security.Cryptography;
using System.Text;

using Calanggo.Application.Common.Results;
using Calanggo.Application.Interfaces;
using Calanggo.Application.UseCases.GetUrlStatistics;
using Calanggo.Domain.Entities;

using Microsoft.Extensions.Logging;

namespace Calanggo.Infrastructure.Services;

public class UrlShortenerService : IUrlShortenerService
{
    private readonly ILogger<UrlShortenerService> _logger;
    private readonly UnitOfWork _unitOfWork;
    private readonly IMemoryCacheService _memoryCacheService;

    public UrlShortenerService(UnitOfWork unitOfWork, IMemoryCacheService memoryCacheService, ILogger<UrlShortenerService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _memoryCacheService = memoryCacheService;
    }

    public async Task<Result<ShortenedUrl>> CreateShortenedUrl(string originalUrl, DateTime? expiresAt = null)
    {
        ShortenedUrl shortenedUrl = new(originalUrl, expiresAt: expiresAt);
        await _unitOfWork.ShortenedUrlRepository.AddAsync(shortenedUrl);
        await _unitOfWork.Commit();

        return Result<ShortenedUrl>.Success(shortenedUrl);
    }

    public async Task<Result<ShortenedUrl>> GetShortenedUrl(string shortCode, string ipAddress, string userAgent, string referer)
    {
        var shortenedUrl = await GetShortenedUrlFromCacheOrDatabase(shortCode);
        if (shortenedUrl is null || shortenedUrl.IsExpired())
        {
            _logger.LogError("The short code provided does not exist or has expired: {ShortCode}", shortCode);
            return Result<ShortenedUrl>.Failure(new Error(404, "Shortened URL not found or has expired"));
        }

        shortenedUrl.Statistics.AddClick(ipAddress, userAgent, referer);
        _memoryCacheService.Set(shortCode, shortenedUrl, TimeSpan.FromMinutes(10));

        await _unitOfWork.Commit();
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

        shortenedUrl ??= await _unitOfWork.ShortenedUrlRepository.GetByShortCodeAsync(shortCode);
        return shortenedUrl;
    }

    #endregion
}