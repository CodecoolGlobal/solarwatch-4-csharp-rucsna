using SolarWatch.Model;

namespace SolarWatch.Service.Repository;

public interface ICityService
{
    Task<IEnumerable<City>?> GetCityByNameAsync(string cityName);
    Task<IEnumerable<City>?> GetAllCitiesAsync();
    Task<City?> CreateCityAsync(Guid id, string name, Coordinate coordinate, string country, string? state);
}