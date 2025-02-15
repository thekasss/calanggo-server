using Calanggo.Application.Common.Results;
using Calanggo.Application.UseCases.GetUrlStatistics;
using Calanggo.Domain.Entities;

namespace Calanggo.Application.Interfaces;

public interface IUrlShortenerService
{
    Task<Result<ShortenedUrl>> CreateShortenedUrl(string originalUrl, DateTime? expiresAt = null);
    Task<Result<ShortenedUrl>> GetShortenedUrl(string shortCode, string ipAddress, string userAgent, string referer);
    Task<Result<UrlStatisticsResponse>> GetUrlStatistics(string shortCode);
}