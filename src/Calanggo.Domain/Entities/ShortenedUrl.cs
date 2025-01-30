namespace Calanggo.Domain.Entities;

public class ShortenedUrl : IBaseEntity
{
    public string OriginalUrl { get; private set; }
    public string ShortCode { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ExpiresAt { get; }
    public bool IsActive { get; private set; }
    public string? CreatedBy { get; private set; }
    public UrlStatistics Statistics { get; private set; }
    public Guid Id { get; init; }

    protected ShortenedUrl() { }

    public ShortenedUrl(string originalUrl, string shortCode, string? createdBy = null, DateTime? expiresAt = null)
    {
        ArgumentNullException.ThrowIfNull(originalUrl);
        ArgumentNullException.ThrowIfNull(shortCode);

        Id = Guid.NewGuid();
        OriginalUrl = originalUrl;
        ShortCode = shortCode;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = expiresAt;
        IsActive = true;
        CreatedBy = createdBy;
        Statistics = new UrlStatistics(this);
    }

    public bool IsExpired()
    {
        return ExpiresAt.HasValue && ExpiresAt.Value < DateTime.UtcNow;
    }
}