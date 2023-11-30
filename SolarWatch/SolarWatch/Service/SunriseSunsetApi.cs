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
        throw new NotImplementedException();
    }
}