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
    
    public Task<string> GetCityDataAsync(string cityName)
    {
        throw new NotImplementedException();
    }
}