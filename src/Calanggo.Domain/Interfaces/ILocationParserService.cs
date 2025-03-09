namespace Calanggo.Domain.Interfaces;

public interface ILocationParserService
{
    (string Country, string Region, string City) Parse(string ipAddress);
    Task<(string Country, string Region, string City)> ParseAsync(string ipAddress, CancellationToken cancellationToken = default);
}