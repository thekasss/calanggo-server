namespace Calango.Domain.Entities;

public class UrlStatistics : IBaseEntity
{
    public Guid ShortenedUrlId { get; private set; }
    public ShortenedUrl ShortenedUrl { get; private set; }
    
    // Estatísticas gerais
    public int TotalClicks { get; private set; }
    public DateTime? LastClickedAt { get; private set; }
    public DateTime? FirstClickedAt { get; private set; }
    public Guid Id { get; init; }

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
    }
}