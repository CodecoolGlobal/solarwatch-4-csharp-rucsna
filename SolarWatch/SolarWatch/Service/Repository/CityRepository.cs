using Microsoft.EntityFrameworkCore;
using SolarWatch.Data;
using SolarWatch.Model;

namespace SolarWatch.Service.Repository;

public class CityRepository : ICityRepository
{
    private readonly SolarDbContext _dbContext;
    private readonly ILogger<CityRepository> _logger;

    public CityRepository(SolarDbContext dbContext, ILogger<CityRepository> logger)
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
    
    public async Task<City?> CreateCityAsync(City newCity)
    {
        var cityInDb =
            await _dbContext.Cities.FirstOrDefaultAsync(city => city.Name == newCity.Name && city.Coordinate == newCity.Coordinate);
        if (cityInDb != null)
        {
            return null;
        }
        
        // when adding a new city, coordinates could be get from Geocoding API automatically
        await _dbContext.Cities.AddAsync(newCity);
        await _dbContext.SaveChangesAsync();
        return newCity;
    }

    public async Task UpdateCityAsync(Guid cityId, City updateRequest)
    {
        var cityInDb = await _dbContext.Cities.FindAsync(cityId);
        if (cityInDb == null)
        {
            _logger.LogError("No city was found in the database with id: {cityId}", cityId);
            throw new Exception($"No city was found in the database with id: {cityId}");
        }

        cityInDb.Update(updateRequest);
        _dbContext.Update(cityInDb);
        await _dbContext.SaveChangesAsync();
    }
}