namespace SolarWatch.Service;

public class GeocodingApi : ICityDataProvider
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<GeocodingApi> _logger;
    private readonly IConfiguration _configuration;

    public GeocodingApi(IHttpClientFactory clientFactory, ILogger<GeocodingApi> logger, IConfiguration configuration)
    {
        _clientFactory = clientFactory;
        _logger = logger;
        _configuration = configuration;
    }
    
    public async Task<string> GetCityDataAsync(string cityName)
    {
        var apiKey = _configuration["GeocodingApiKey"];

        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={cityName}&appid={apiKey}";

        var client = _clientFactory.CreateClient();
        _logger.LogInformation("Calling OpenWeather API with url: {url}", url);

        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}