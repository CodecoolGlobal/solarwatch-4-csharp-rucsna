using Microsoft.EntityFrameworkCore;
using SolarWatch.Data;
using SolarWatch.Model;

namespace SolarWatch.Service.Repository;

public class CityService : ICityService
{
    private readonly SolarDbContext _dbContext;
    private readonly ILogger<CityService> _logger;

    public CityService(SolarDbContext dbContext, ILogger<CityService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<IEnumerable<City>?> GetCityByNameAsync(string cityName)
    {
        var cityList = await _dbContext.Cities
            .Include(c => c.Coordinate)
            .Where(city => city.Name.ToLower() == cityName)
            .ToListAsync();
        return cityList.Count < 1 ? null : cityList;
    }

    public async Task<IEnumerable<City>?> GetAllCitiesAsync()
    {
        var allCities = await _dbContext.Cities
            .Include(c => c.Coordinate)
            .ToListAsync();
        return allCities.Count < 1 ? null : allCities;
    }
    
    public async Task<City?> CreateCityAsync(Guid id, string name, Coordinate coordinate, string country, string? state)
    {
        var cityInDb =
            await _dbContext.Cities.FirstOrDefaultAsync(city => city.Name == name && city.Coordinate == coordinate);
        if (cityInDb != null)
        {
            return null;
        }
        var newCity = new City { CityId = id, Name = name, Coordinate = coordinate, Country = country, State = state };
        // when adding a new city, coordinates could be get from Geocoding API automatically

        await _dbContext.Cities.AddAsync(newCity);
        await _dbContext.SaveChangesAsync();
        return newCity;
    }
}