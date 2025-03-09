namespace Calanggo.Application.UseCases.GetUrlStatistics;

public record LocationMetricResponse
{
    public required string Country { get; set; }
    public required string Region { get; set; }
    public required string City { get; set; }
    public int Clicks { get; set; }
}