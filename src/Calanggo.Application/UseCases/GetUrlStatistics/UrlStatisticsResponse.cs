namespace Calanggo.Application.UseCases.GetUrlStatistics;

public record UrlStatisticsResponse
{
    public int TotalClicks { get; set; }
    public DateTime? LastClickedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<LocationMetricResponse> LocationMetrics { get; set; } = [];
    public List<DeviceMetricResponse> DeviceMetrics { get; set; } = [];
}