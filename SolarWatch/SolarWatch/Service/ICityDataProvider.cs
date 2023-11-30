namespace SolarWatch.Service;

public interface ICityDataProvider
{
    Task<string> GetCityDataAsync(string cityName);
}