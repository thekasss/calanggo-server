using Pingu.Application.Common.Results;
using Pingu.Core.Domain.Entities;

namespace Pingu.Application.Interfaces;

public interface IUrlShortenerService
{
    Task<Result<ShortenedUrl>> CreateShortenedUrl(string originalUrl, DateTime? expiresAt = null);
    Task<Result<ShortenedUrl>> GetShortenedUrl(string shortCode);
}