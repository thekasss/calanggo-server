namespace Calanggo.Domain.Entities;

public class DeviceMetric : IBaseEntity
{
    public Guid Id { get; init; }
    public Guid UrlStatisticsId { get; private set; }
    public virtual UrlStatistics UrlStatistics { get; private set; }
    
    public string DeviceType { get; private set; }
    public string Browser { get; private set; }
    public int Clicks { get; private set; }

    protected DeviceMetric() { }

    public DeviceMetric(Guid urlStatisticsId, ClickEvent clickEvent)
    {
        UrlStatisticsId = urlStatisticsId;
        
        DeviceType = clickEvent.DeviceType;
        Browser = clickEvent.Browser;
        Clicks = 1;
    }

    public void IncrementClicks()
    {
        Clicks++;
    }
}