using Calanggo.Domain.Entities;
using Calanggo.Domain.Interfaces;

namespace Calanggo.Domain.Factories;

public class ClickEventFactory(IUserAgentParserService userAgentParser, ILocationParserService locationParser)
{
    private readonly IUserAgentParserService _userAgentParser = userAgentParser;
    private readonly ILocationParserService _locationParser = locationParser;

    // public ClickEvent Create(Guid urlStatisticsId, string ipAddress, string userAgent, string referer)
    // {
    //     return new ClickEvent(
    //         urlStatisticsId,
    //         ipAddress,
    //         userAgent,
    //         referer,
    //         _userAgentParser,
    //         _locationParser);
    // }
}