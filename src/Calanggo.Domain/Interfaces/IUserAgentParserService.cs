namespace Calanggo.Domain.Interfaces;

public interface IUserAgentParserService
{
    (string DeviceType, string Browser, string OperatingSystem) Parse(string userAgent);
}