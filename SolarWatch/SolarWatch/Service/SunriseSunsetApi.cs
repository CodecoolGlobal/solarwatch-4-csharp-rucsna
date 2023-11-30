using System.Net;
using SolarWatch.Client;

namespace SolarWatch.Service;

public class SunriseSunsetApi : ISolarDataProvider
{
    private readonly ISolarWatchClient _solarWatchClient;
    private readonly ILogger<SunriseSunsetApi> _logger;

    public SunriseSunsetApi(ISolarWatchClient solarWatchClient, ILogger<SunriseSunsetApi> logger)
    {
        _solarWatchClient = solarWatchClient;
        _logger = logger;
    }

    public async Task<string> GetSolarDataAsync(float lat, float lon, DateTime date)
    {
        var formattedDate = date.ToString("yyyy-MM-dd");
        
        var url = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}&date={formattedDate}";
       
        _logger.LogInformation("Calling Sunrise/Sunset API from url: {url}", url);

        var client = _solarWatchClient.GetClient();
        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}