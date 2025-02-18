namespace Calanggo.Application.UseCases.GetUrlStatistics;

public record DeviceMetricResponse
{
    public string DeviceType { get; set; } = string.Empty;
    public string Browser { get; set; } = string.Empty;
    public int Clicks { get; set; }
}