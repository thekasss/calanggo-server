namespace Calanggo.Domain.Entities;

public class ClickEvent : IBaseEntity
{
    public Guid Id { get; init; }
    public Guid UrlStatisticsId { get; private set; }
    public virtual UrlStatistics UrlStatistics { get; private set; }

    public DateTime ClickedAt { get; private set; }
    public string IpAddress { get; private set; }
    public string UserAgent { get; private set; }
    public string Referer { get; private set; }

    // Informações de localização
    public string Country { get; private set; }
    public string Region { get; private set; }
    public string City { get; private set; }

    // Informações do dispositivo
    public string DeviceType { get; private set; }
    public string Browser { get; private set; }
    public string OperatingSystem { get; private set; }

    protected ClickEvent() { }

    public ClickEvent(UrlStatistics statistics, string ipAddress, string userAgent, string referer)
    {
        Id = Guid.NewGuid();
        UrlStatistics = statistics;
        UrlStatisticsId = statistics.Id;
        ClickedAt = DateTime.UtcNow;

        IpAddress = ipAddress;
        UserAgent = userAgent;
        Referer = referer;

        // TODO: implementar logica para obter informações de dispositivo
        ParseUserAgent(userAgent);

        // TODO: implementar logica para obter informações de localização
        ParseLocation(ipAddress);
    }

    private void ParseUserAgent(string userAgent)
    {

    }

    private void ParseLocation(string ipAddress)
    {

    }
}