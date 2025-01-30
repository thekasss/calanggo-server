using Calanggo.Application.Common.Results;

using Calanggo.Domain.Entities;

namespace Calanggo.Application.Interfaces;

public interface IUrlShortenerService
{
    Task<Result<ShortenedUrl>> CreateShortenedUrl(string originalUrl, DateTime? expiresAt = null);
    Task<Result<ShortenedUrl>> GetShortenedUrl(string shortCode);
}