using Calango.Application.Common.Results;
using Calango.Domain.Entities;

namespace Calango.Application.Interfaces;

public interface IUrlShortenerService
{
    Task<Result<ShortenedUrl>> CreateShortenedUrl(string originalUrl, DateTime? expiresAt = null);
    Task<Result<ShortenedUrl>> GetShortenedUrl(string shortCode);
}