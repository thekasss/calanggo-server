using System.Text.RegularExpressions;
using Calanggo.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Calanggo.Infrastructure.Services;

public class UserAgentParserService(ILogger<UserAgentParserService> logger) : IUserAgentParserService
{
    private readonly ILogger<UserAgentParserService> _logger = logger;

    public (string DeviceType, string Browser, string OperatingSystem) Parse(string userAgent)
    {
        if (string.IsNullOrEmpty(userAgent))
        {
            _logger.LogWarning("Empty user agent string received");
            return ("Unknown", "Unknown", "Unknown");
        }

        try
        {
            return (
                DeviceType: DetectDeviceType(userAgent),
                Browser: DetectBrowser(userAgent),
                OperatingSystem: DetectOperatingSystem(userAgent)
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error parsing user agent: {UserAgent}", userAgent);
            return ("Unknown", "Unknown", "Unknown");
        }
    }

    # region [Private Methods]

    private string DetectDeviceType(string userAgent)
    {
        if (Regex.IsMatch(userAgent, @"(?i)tablet|ipad"))
            return "Tablet";

        if (Regex.IsMatch(userAgent, @"(?i)mobile|android|iphone|ipod"))
            return "Mobile";

        return "Desktop";
    }

    private string DetectBrowser(string userAgent)
    {
        if (Regex.IsMatch(userAgent, @"(?i)chrome") && !Regex.IsMatch(userAgent, @"(?i)edg|opr"))
            return "Chrome";

        if (Regex.IsMatch(userAgent, @"(?i)firefox"))
            return "Firefox";

        if (Regex.IsMatch(userAgent, @"(?i)safari") && !Regex.IsMatch(userAgent, @"(?i)chrome"))
            return "Safari";

        if (Regex.IsMatch(userAgent, @"(?i)edg"))
            return "Edge";

        if (Regex.IsMatch(userAgent, @"(?i)opr|opera"))
            return "Opera";

        if (Regex.IsMatch(userAgent, @"(?i)msie|trident"))
            return "Internet Explorer";

        return "Other";
    }

    private string DetectOperatingSystem(string userAgent)
    {
        if (Regex.IsMatch(userAgent, @"(?i)windows"))
            return "Windows";

        if (Regex.IsMatch(userAgent, @"(?i)android"))
            return "Android";

        if (Regex.IsMatch(userAgent, @"(?i)iphone|ipad|ipod|ios"))
            return "iOS";

        if (Regex.IsMatch(userAgent, @"(?i)mac os|macintosh"))
            return "macOS";

        if (Regex.IsMatch(userAgent, @"(?i)linux"))
            return "Linux";

        return "Other";
    }

    # endregion
}
