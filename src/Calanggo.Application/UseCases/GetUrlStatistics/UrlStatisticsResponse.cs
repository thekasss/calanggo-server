namespace Calanggo.Application.UseCases.GetUrlStatistics;

public class UrlStatisticsResponse
{
    public int TotalClicks { get; set; }
    public DateTime? LastClickedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<LocationMetricResponse> LocationMetrics { get; set; } = new();
    public List<DeviceMetricResponse> DeviceMetrics { get; set; } = new();
}