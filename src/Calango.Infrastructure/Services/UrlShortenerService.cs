using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using Calango.Application.Common.Results;
using Calango.Application.Interfaces;
using Calango.Domain.Entities;
using Calango.Domain.Interfaces.Repositories;

namespace Calango.Infrastructure.Services;

public class UrlShortenerService(IShortenedUrlRepository shortenedUrlRepository, IMemoryCacheService memoryCacheService, ILogger<UrlShortenerService> logger) 
    : IUrlShortenerService
{
    private const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-";
    private const int ShortCodeLength = 7;
    private readonly ILogger<UrlShortenerService> _logger = logger;
    private readonly IMemoryCacheService _memoryCacheService = memoryCacheService;
    private readonly IShortenedUrlRepository _shortenedUrlRepository = shortenedUrlRepository;

    public async Task<Result<ShortenedUrl>> CreateShortenedUrl(string originalUrl, DateTime? expiresAt = null)
    {
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
        _memoryCacheService.TryGet(shortCode, out ShortenedUrl? shortenedUrl);
        shortenedUrl ??= await _shortenedUrlRepository.FindAsync(entity => entity.ShortCode == shortCode, true);
        if (shortenedUrl is not null) _memoryCacheService.Set(shortCode, shortenedUrl);
        
        return shortenedUrl == null
            ? Result<ShortenedUrl>.Failure(new Error(204, "The provided short code does not exist."))
            : Result<ShortenedUrl>.Success(shortenedUrl);
    }

    # region [Private Methods]

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