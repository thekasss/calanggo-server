namespace Calanggo.Application.UseCases.GetUrlStatistics;

public class LocationMetricResponse
{
    public string Country { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int Clicks { get; set; }
}