using SolarWatch.Model;

namespace SolarWatch.Service.Repository;

public interface ICityRepository
{
    Task<IEnumerable<City>?> GetCityByNameAsync(string cityName);
    Task<IEnumerable<City>?> GetAllCitiesAsync();
    Task<City?> CreateCityAsync(City newCity);
    Task UpdateCityAsync(Guid cityId, City updateRequest);
}