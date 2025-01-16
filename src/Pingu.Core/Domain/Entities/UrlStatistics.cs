namespace Pingu.Core.Domain.Entities;

public class UrlStatistics
{
    public Guid Id { get; private set; }
    public Guid ShortenedUrlId { get; private set; }
    public ShortenedUrl ShortenedUrl { get; private set; }

    // TODO:
    // Estatísticas gerais
    // public int TotalClicks { get; private set; }
    // public DateTime LastClickedAt { get; private set; }
    // public DateTime FirstClickedAt { get; private set; }

    // // Estatísticas por período
    // public int ClicksLast24Hours { get; private set; }
    // public int ClicksLast7Days { get; private set; }
    // public int ClicksLast30Days { get; private set; }

    protected UrlStatistics() { }

    public UrlStatistics(ShortenedUrl shortenedUrl)
    {
        ArgumentNullException.ThrowIfNull(shortenedUrl);

        Id = Guid.NewGuid();
        ShortenedUrl = shortenedUrl;
        ShortenedUrlId = shortenedUrl.Id;

        // TODO:
        // TotalClicks = 0;
        // BrowserStats = new Dictionary<string, int>();
        // DeviceStats = new Dictionary<string, int>();
        // CountryStats = new Dictionary<string, int>();
    }
}