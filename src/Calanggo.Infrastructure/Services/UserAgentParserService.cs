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

    #region [Private Methods]

    private string DetectDeviceType(string userAgent)
    {
        return userAgent switch
        {
            var ua when Regex.IsMatch(ua, @"(?i)tablet|ipad") => "Tablet",
            var ua when Regex.IsMatch(ua, @"(?i)mobile|android|iphone|ipod") => "Mobile",
            _ => "Desktop"
        };
    }

    private string DetectBrowser(string userAgent)
    {
        return userAgent switch
        {
            var ua when Regex.IsMatch(ua, @"(?i)chrome") && !Regex.IsMatch(ua, @"(?i)edg|opr") => "Chrome",
            var ua when Regex.IsMatch(ua, @"(?i)firefox") => "Firefox",
            var ua when Regex.IsMatch(ua, @"(?i)safari") && !Regex.IsMatch(ua, @"(?i)chrome") => "Safari",
            var ua when Regex.IsMatch(ua, @"(?i)edg") => "Edge",
            var ua when Regex.IsMatch(ua, @"(?i)opr|opera") => "Opera",
            var ua when Regex.IsMatch(ua, @"(?i)msie|trident") => "Internet Explorer",
            _ => "Unknown"
        };
    }

    private string DetectOperatingSystem(string userAgent)
    {
        return userAgent switch
        {
            var ua when Regex.IsMatch(ua, @"(?i)windows") => "Windows",
            var ua when Regex.IsMatch(ua, @"(?i)android") => "Android",
            var ua when Regex.IsMatch(ua, @"(?i)iphone|ipad|ipod|ios") => "iOS",
            var ua when Regex.IsMatch(ua, @"(?i)mac os|macintosh") => "macOS",
            var ua when Regex.IsMatch(ua, @"(?i)linux") => "Linux",
            _ => "Unknown"
        };
    }

    # endregion
}
