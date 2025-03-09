namespace Calanggo.Application.UseCases.ShortenUrl;

public record ShortenUrlRequest(string OriginalUrl, DateTime? ExpiresAt = null);