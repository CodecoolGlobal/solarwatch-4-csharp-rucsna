namespace SolarWatch.Service;

public class SunriseSunsetApi : ISolarDataProvider
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<SunriseSunsetApi> _logger;

    public SunriseSunsetApi(ILogger<SunriseSunsetApi> logger, IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _clientFactory = clientFactory;
    }

    public async Task<string> GetSolarDataAsync(float lat, float lon, DateTime date)
    {
        var formattedDate = date.ToString("yyyy-MM-dd");
        
        var url = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}&date={formattedDate}";
       
        _logger.LogInformation("Calling Sunrise/Sunset API from url: {url}", url);

        var client = _clientFactory.CreateClient();
        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}