using Calanggo.Domain.Entities;

namespace Calanggo.Application.UseCases.GetUrlStatistics;

public class UrlStatisticsResponse
{
    public int TotalClicks { get; set; }
    public DateTime? LastClickedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<LocationMetricResponse> LocationMetrics { get; set; } = [];
    public List<DeviceMetricResponse> DeviceMetrics { get; set; } = [];

    public UrlStatisticsResponse() { }
    public UrlStatisticsResponse FromStatistics(UrlStatistics statistics)
    {
        TotalClicks = statistics.TotalClicks;
        LastClickedAt = statistics.LastClickedAt;
        CreatedAt = statistics.CreatedAt;
        LocationMetrics = statistics.LocationMetrics?.Select(lm => new LocationMetricResponse
        {
            Country = lm.Country,
            Region = lm.Region,
            City = lm.City,
            Clicks = lm.Clicks
        }).ToList()!;
        DeviceMetrics = statistics.DeviceMetrics?.Select(dm => new DeviceMetricResponse
        {
            DeviceType = dm.DeviceType,
            Browser = dm.Browser,
            Clicks = dm.Clicks
        }).ToList()!;


        return this;
    }
}
