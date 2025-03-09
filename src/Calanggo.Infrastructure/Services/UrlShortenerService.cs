using Calanggo.Application.Common.Results;
using Calanggo.Application.Interfaces;
using Calanggo.Application.Interfaces.CacheService;
using Calanggo.Application.UseCases.GetUrlStatistics;
using Calanggo.Domain.Entities;
using Calanggo.Domain.Factories;

using Microsoft.Extensions.Logging;

namespace Calanggo.Infrastructure.Services;

public class UrlShortenerService : IUrlShortenerService
{
    private readonly ILogger<UrlShortenerService> _logger;
    private readonly UnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly ClickEventFactory _clickEventFactory;

    public UrlShortenerService(UnitOfWork unitOfWork, ICacheService memoryCacheService, ILogger<UrlShortenerService> logger, ClickEventFactory clickEventFactory)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _cacheService = memoryCacheService;
        _clickEventFactory = clickEventFactory;
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

        var clickEvent = _clickEventFactory.Create(shortenedUrl.Statistics.Id, ipAddress, userAgent, referer);
        shortenedUrl.Statistics.AddClick(clickEvent);
        _cacheService.Set(shortCode, shortenedUrl, TimeSpan.FromMinutes(10));

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

        var response = new UrlStatisticsResponse().FromStatistics(shortenedUrl.Statistics);
        return Result<UrlStatisticsResponse>.Success(response);
    }

    #region [Private Methods]

    private async Task<ShortenedUrl?> GetShortenedUrlFromCacheOrDatabase(string shortCode)
    {
        if (_cacheService.TryGet(shortCode, out ShortenedUrl? shortenedUrl))
            return shortenedUrl;

        shortenedUrl ??= await _unitOfWork.ShortenedUrlRepository.GetByShortCodeAsync(shortCode);
        return shortenedUrl;
    }

    #endregion
}