namespace Calango.Application.UseCases.ShortenUrl;

public record ShortenUrlResponse(string OriginalUrl, string ShortenedUrl, string Code, DateTime? ExpiresAt);