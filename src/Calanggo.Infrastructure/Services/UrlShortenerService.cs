using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using Calanggo.Application.Common.Results;
using Calanggo.Application.Interfaces;
using Calanggo.Domain.Entities;
using Calanggo.Domain.Interfaces.Repositories;

namespace Calango.Infrastructure.Services;

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
        if (Uri.TryCreate(originalUrl, UriKind.Absolute, out _) == false)
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

    public async Task<Result<ShortenedUrl>> GetShortenedUrl(string shortCode)
    {
        var shortenedUrl = await GetShortenedUrlFromCacheOrDatabase(shortCode);
        if (shortenedUrl is null || shortenedUrl.IsExpired())
        {
            _logger.LogError("The short code provided does not exist or has expired: {ShortCode}", shortCode);
            return Result<ShortenedUrl>.Failure(new Error(204));
        }

        await UpdateUrlStatistics(shortenedUrl);
        return Result<ShortenedUrl>.Success(shortenedUrl);
    }

    #region [Private Methods]

    private async Task<ShortenedUrl?> GetShortenedUrlFromCacheOrDatabase(string shortCode)
    {
        if (_memoryCacheService.TryGet(shortCode, out ShortenedUrl? shortenedUrl))
            return shortenedUrl;

        shortenedUrl = await _shortenedUrlRepository.FindAsync(entity => entity.ShortCode == shortCode, true);
        if (shortenedUrl != null) _memoryCacheService.Set(shortCode, shortenedUrl);

        return shortenedUrl;
    }

    private async Task UpdateUrlStatistics(ShortenedUrl shortenedUrl)
    {
        shortenedUrl.Statistics.AddClick();
        _shortenedUrlRepository.UpdateAsync(shortenedUrl);
        await _shortenedUrlRepository.Commit();
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

    #endregion
}