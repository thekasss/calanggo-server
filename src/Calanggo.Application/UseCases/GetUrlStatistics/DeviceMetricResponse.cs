namespace Calanggo.Application.UseCases.GetUrlStatistics;

public record DeviceMetricResponse
{
    public required string DeviceType { get; set; }
    public required string Browser { get; set; }
    public int Clicks { get; set; }
}