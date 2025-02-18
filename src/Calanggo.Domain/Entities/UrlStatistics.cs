namespace Calanggo.Domain.Entities;

public class UrlStatistics : IBaseEntity
{
    public Guid Id { get; init; }
    public Guid ShortenedUrlId { get; private set; }
    public virtual ShortenedUrl ShortenedUrl { get; private set; }

    // Estatísticas gerais
    public int TotalClicks { get; private set; } = 0;
    public DateTime? LastClickedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Coleção de clicks detalhados
    public virtual ICollection<ClickEvent> ClickEvents { get; private set; } = [];

    // Métricas agregadas por localização
    public virtual ICollection<LocationMetric> LocationMetrics { get; private set; } = [];

    // Métricas agregadas por dispositivo
    public virtual ICollection<DeviceMetric> DeviceMetrics { get; private set; } = [];

    protected UrlStatistics() { }

    public UrlStatistics(ShortenedUrl shortenedUrl)
    {
        ArgumentNullException.ThrowIfNull(shortenedUrl);

        Id = Guid.NewGuid();
        ShortenedUrl = shortenedUrl;
        ShortenedUrlId = shortenedUrl.Id;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddClick(string ipAddress, string userAgent, string referer)
    {
        var clickEvent = new ClickEvent(this, ipAddress, userAgent, referer);
        ClickEvents.Add(clickEvent);

        TotalClicks++;
        LastClickedAt = clickEvent.ClickedAt;

        UpdateLocationMetrics(clickEvent);
        UpdateDeviceMetrics(clickEvent);
    }

    private void UpdateLocationMetrics(ClickEvent clickEvent)
    {
        var metric = LocationMetrics.FirstOrDefault(lm => lm.Country == clickEvent.Country
            && lm.Region == clickEvent.Region
            && lm.City == clickEvent.City);

        if (metric == null)
        {
            metric = new LocationMetric(this, clickEvent);
            LocationMetrics.Add(metric);
        }
        else
        {
            metric.IncrementClicks();
        }
    }

    private void UpdateDeviceMetrics(ClickEvent clickEvent)
    {
        var metric = DeviceMetrics.FirstOrDefault(dm => dm.DeviceType == clickEvent.DeviceType
            && dm.Browser == clickEvent.Browser);

        if (metric == null)
        {
            metric = new DeviceMetric(this, clickEvent);
            DeviceMetrics.Add(metric);
        }
        else
        {
            metric.IncrementClicks();
        }
    }
}