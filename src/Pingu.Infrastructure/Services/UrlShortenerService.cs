using System.Security.Cryptography;
using System.Text;

using Microsoft.Extensions.Logging;

using Pingu.Application.Common.Results;
using Pingu.Application.Interfaces;
using Pingu.Core.Domain.Entities;
using Pingu.Domain.Interfaces.Repositories;

namespace Pingu.Infrastructure.Services;

public class UrlShortenerService(
    IShortenedUrlRepository shortenedUrlRepository,
    IMemoryCacheService memoryCacheService,
    ILogger<UrlShortenerService> logger) : IUrlShortenerService
{
    private readonly IShortenedUrlRepository _shortenedUrlRepository = shortenedUrlRepository;
    private readonly ILogger<UrlShortenerService> _logger = logger;
    private readonly IMemoryCacheService _memoryCacheService = memoryCacheService;
    private const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-";
    private const int ShortCodeLength = 7;

    public async Task<Result<ShortenedUrl>> CreateShortenedUrl(string originalUrl, string? createdBy = null, DateTime? expiresAt = null)
    {
        if (Uri.TryCreate(originalUrl, UriKind.Absolute, out _) == false)
        {
            _logger.LogError("The provided URL is not valid: {OriginalUrl}", originalUrl);
            return Result<ShortenedUrl>.Failure(new Error(400, "The provided URL is not valid."));
        }

        var shortCode = GenerateShortCode();
        var shortenedUrl = new ShortenedUrl(originalUrl, shortCode, createdBy, expiresAt);

        await _shortenedUrlRepository.AddAsync(shortenedUrl);
        await _shortenedUrlRepository.Commit();

        return Result<ShortenedUrl>.Success(shortenedUrl);
    }

    public async Task<Result<ShortenedUrl>> GetShortenedUrl(string shortCode)
    {
        _logger.LogInformation("Getting shortened URL with short code: {ShortCode}", shortCode);
        _memoryCacheService.TryGet(shortCode, out ShortenedUrl? shortenedUrl);
        shortenedUrl ??= await _shortenedUrlRepository.FindAsync(entity => entity.ShortCode == shortCode, true);
        if (shortenedUrl is not null) _memoryCacheService.Set(shortCode, shortenedUrl);

        return shortenedUrl == null
            ? Result<ShortenedUrl>.Failure(new Error(404, "The provided short code does not exist."))
            : Result<ShortenedUrl>.Success(shortenedUrl);
    }

    # region [Private Methods]
    private static string GenerateShortCode()
    {
        using var rng = RandomNumberGenerator.Create();
        var result = new StringBuilder(ShortCodeLength);
        var bytes = new byte[ShortCodeLength];

        rng.GetBytes(bytes);
        for (int i = 0; i < ShortCodeLength; i++)
        {
            result.Append(AllowedChars[bytes[i] % AllowedChars.Length]);
        }

        return result.ToString();
    }
    #endregion
}