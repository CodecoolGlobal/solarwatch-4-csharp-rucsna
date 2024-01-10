using System.Text.Json;
using SolarWatch.Model;

namespace SolarWatch.Service.Processors;

public class CityJsonProcessor : ICityJsonProcessor
{
    private readonly ILogger<CityJsonProcessor> _logger;

    public CityJsonProcessor(ILogger<CityJsonProcessor> logger)
    {
        _logger = logger;
    }

    public City Process(string data)
    {
        var json = JsonDocument.Parse(data);
        var response = json.RootElement.EnumerateArray().FirstOrDefault();
        var cityCoordinate = new Coordinate
        {
            Latitude = response.GetProperty("lat").GetSingle(),
            Longitude = response.GetProperty("lon").GetSingle()
        };
        
        var city = new City
        {
            CityId = Guid.NewGuid(),
            Name = response.GetProperty("name").ToString(),
            Coordinate = cityCoordinate,
            Country = response.GetProperty("country").ToString(),
            State = response.TryGetProperty("state", out var stateElement) ? stateElement.GetString() : string.Empty
        };
        _logger.LogInformation("New city is successfully created.");

        return city;
    }
}