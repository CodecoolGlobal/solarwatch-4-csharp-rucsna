namespace SolarWatch.Service;

public interface ISolarDataProvider
{
    Task<string> GetSolarDataAsync(float lat, float lon, DateTime date);
}