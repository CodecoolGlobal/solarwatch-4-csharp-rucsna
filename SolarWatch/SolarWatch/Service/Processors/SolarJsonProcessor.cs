using System.Text.Json;
using SolarWatch.Model;

namespace SolarWatch.Service.Processors;

public class SolarJsonProcessor : ISolarJsonProcessor
{
    private readonly ILogger<SolarJsonProcessor> _logger;

    public SolarJsonProcessor(ILogger<SolarJsonProcessor> logger)
    {
        _logger = logger;
    }

    public SolarData Process(string data, DateTime date)
    {
        var json = JsonDocument.Parse(data);
        var results = json.RootElement.GetProperty("results");

        var solarResult = new SolarData
        {
            Date = date,
            Sunrise = results.GetProperty("sunrise").ToString(),
            Sunset = results.GetProperty("sunset").ToString()
        };
        _logger.LogInformation("New solarData is successfully created.");

        return solarResult;
    }
}