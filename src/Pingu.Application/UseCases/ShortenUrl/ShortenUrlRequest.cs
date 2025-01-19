namespace Pingu.Application.UseCases.ShortenUrl;

public record ShortenUrlRequest(string OriginalUrl, string? CreatedBy, DateTime? ExpiresAt);