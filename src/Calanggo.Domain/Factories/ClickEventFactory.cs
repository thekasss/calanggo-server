using Calanggo.Domain.Entities;
using Calanggo.Domain.Interfaces;

namespace Calanggo.Domain.Factories;

public class ClickEventFactory(IUserAgentParserService userAgentParser, ILocationParserService locationParser)
{
    private readonly IUserAgentParserService _userAgentParser = userAgentParser;
    private readonly ILocationParserService _locationParser = locationParser;

    public ClickEvent Create(Guid urlStatisticsId, string ipAddress, string userAgent, string referer)
    {
        var clickEvent = new ClickEvent(urlStatisticsId, ipAddress, userAgent, referer);

        var (deviceType, browser, operatingSystem) = _userAgentParser.Parse(userAgent);
        clickEvent.SetDeviceInformation(deviceType, browser, operatingSystem);

        var (country, region, city) = _locationParser.Parse(ipAddress);
        clickEvent.SetLocation(country, region, city);

        return clickEvent;
    }
}