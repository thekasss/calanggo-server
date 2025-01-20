using Pingu.Application.Common.Results;
using Pingu.Core.Domain.Entities;

namespace Pingu.Application.Interfaces;

public interface IUrlShortenerService
{
    Task<Result<ShortenedUrl>> CreateShortenedUrl(string originalUrl, string? createdBy = null, DateTime? expiresAt = null);
    string GenerateShortCode();
}