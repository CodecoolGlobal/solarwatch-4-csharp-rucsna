using System.Net;
using SolarWatch.Client;

namespace SolarWatch.Service;

public class GeocodingApi : ICityDataProvider
{
    private readonly ISolarWatchClient _solarWatchClient;
    private readonly ILogger<GeocodingApi> _logger;

    public GeocodingApi(ISolarWatchClient solarWatchClient, ILogger<GeocodingApi> logger)
    {
        _solarWatchClient = solarWatchClient;
        _logger = logger;
    }
    
    public async Task<string> GetCityDataAsync(string cityName)
    {
        var apiKey = "";

        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={cityName}&appid={apiKey}";
        
        var client = _solarWatchClient.GetClient();
        _logger.LogInformation("Calling OpenWeather API with url: {url}", url);

        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}