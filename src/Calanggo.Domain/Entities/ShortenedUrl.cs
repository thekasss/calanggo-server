using System.Security.Cryptography;
using System.Text;

namespace Calanggo.Domain.Entities;

public class ShortenedUrl : IBaseEntity
{
    private const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const int ShortCodeLength = 7;

    public Guid Id { get; init; }
    public string OriginalUrl { get; private set; }
    public string ShortCode { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ExpiresAt { get; }
    public bool IsActive { get; private set; }
    public string? CreatedBy { get; private set; }
    public virtual UrlStatistics Statistics { get; private set; }

    protected ShortenedUrl() { }

    public ShortenedUrl(string originalUrl, DateTime? expiresAt, string? createdBy = null)
    {
        #region [Validation]

        ArgumentNullException.ThrowIfNull(originalUrl);
        if (expiresAt.HasValue && expiresAt.Value < DateTime.UtcNow)
        {
            throw new ArgumentException("The expiration date must be in the future", nameof(expiresAt));
        }

        if (Uri.TryCreate(originalUrl, UriKind.Absolute, out _))
        {
            throw new ArgumentException("The provided URL is not valid", nameof(originalUrl));
        }
        
        #endregion

        Id = Guid.NewGuid();
        OriginalUrl = originalUrl;
        ShortCode = GenerateShortCode();
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = expiresAt ?? DateTime.UtcNow.AddDays(7);
        IsActive = true;
        CreatedBy = createdBy;
        Statistics = new UrlStatistics(this);
    }

    public bool IsExpired()
    {
        return ExpiresAt.HasValue && ExpiresAt.Value < DateTime.UtcNow;
    }

    #region [Private Methods]

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