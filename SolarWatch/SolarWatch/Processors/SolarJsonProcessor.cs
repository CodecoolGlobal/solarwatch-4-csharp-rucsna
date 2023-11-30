using System.Text.Json;
using SolarWatch.Model;

namespace SolarWatch.Processors;

public class SolarJsonProcessor : ISolarJsonProcessor
{
    public SolarData Process(string data, string city, DateTime date)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement results = json.RootElement.GetProperty("results");

        SolarData solarResult = new SolarData
        {
            City = city,
            Date = date,
            Sunrise = results.GetProperty("sunrise").ToString(),
            Sunset = results.GetProperty("sunset").ToString()
        };

        return solarResult;
    }
}