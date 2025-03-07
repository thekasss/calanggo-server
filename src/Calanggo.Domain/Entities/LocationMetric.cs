namespace Calanggo.Domain.Entities;

public class LocationMetric : IBaseEntity
{
    public Guid Id { get; init; }
    public Guid UrlStatisticsId { get; private set; }
    public virtual UrlStatistics UrlStatistics { get; private set; }

    public string Country { get; private set; }
    public string Region { get; private set; }
    public string City { get; private set; }
    public int Clicks { get; private set; }

    protected LocationMetric() { }

    public LocationMetric(Guid urlStatisticsId, ClickEvent clickEvent)
    {
        UrlStatisticsId = urlStatisticsId;

        Country = clickEvent.Country;
        Region = clickEvent.Region;
        City = clickEvent.City;
        Clicks = 1;
    }

    public void IncrementClicks()
    {
        Clicks++;
    }
}