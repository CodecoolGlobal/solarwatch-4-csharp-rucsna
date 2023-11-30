using System.Text.Json;
using SolarWatch.Model;

namespace SolarWatch.Processors;

public class CityJsonProcessor : ICityJsonProcessor
{
    public CityData Process(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement response = json.RootElement.EnumerateArray().FirstOrDefault();

        CityData city = new CityData
        {
            Name = response.GetProperty("name").ToString(),
            Latitude = response.GetProperty("lat").GetSingle(),
            Longitude = response.GetProperty("lon").GetSingle()
        };

        return city;
    }
}