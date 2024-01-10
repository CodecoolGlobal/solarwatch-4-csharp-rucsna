using Microsoft.EntityFrameworkCore;
using SolarWatch.Data;
using SolarWatch.Model;
using SolarWatch.Service.Processors;

namespace SolarWatch.Service.Repository;

public class SolarWatchRepository : ISolarWatchRepository
{
    private readonly SolarDbContext _dbContext;
    private readonly ICityDataProvider _cityDataProvider;
    private readonly ISolarDataProvider _solarDataProvider;
    private readonly ICityJsonProcessor _cityJsonProcessor;
    private readonly ISolarJsonProcessor _solarJsonProcessor;
    private readonly ILogger<SolarWatchRepository> _logger;

    public SolarWatchRepository(SolarDbContext dbContext, ICityDataProvider cityDataProvider,
        ISolarDataProvider solarDataProvider, ICityJsonProcessor cityJsonProcessor,
        ISolarJsonProcessor solarJsonProcessor, ILogger<SolarWatchRepository> logger)
    {
        _dbContext = dbContext;
        _cityDataProvider = cityDataProvider;
        _solarDataProvider = solarDataProvider;
        _cityJsonProcessor = cityJsonProcessor;
        _solarJsonProcessor = solarJsonProcessor;
        _logger = logger;
    }

    public async Task<SolarData?> GetDataAndAddToDbAsync(string cityName, DateTime date)
    {
        var existingSolarDataList = await _dbContext.SolarData.Where(sd => sd.Date == date).ToListAsync();
        //_logger.LogInformation($"Existing solar data detected with date: {existingSolarData?.Date}, and cityID: {existingSolarData?.CityId}");

        var cityInDb = await _dbContext.Cities.Include(c => c.Coordinate)
            .FirstOrDefaultAsync(city => city.Name == cityName);
        
        if (existingSolarDataList.Count == 0)
        {
            _logger.LogInformation("No solar data was found in the database.");
            // 1 - no solar data in db but city is there - get solar data from API with existing city's data, SAVE solar data only
            
            if (cityInDb != null)
            {
                _logger.LogInformation("{cityName} city was found in database", cityInDb.Name);
                return await GetSolarDataFromApi(cityInDb.Coordinate.Latitude, cityInDb.Coordinate.Longitude, date, cityInDb.CityId);
            }
            
            // 2 - no solar data and no city in db - get city AND solar data from API, SAVE both
            _logger.LogInformation("No solar data and no city was found in the database.");
            var cityToSave = await GetCityFromApi(cityName);
            return await GetSolarDataFromApi(cityToSave.Coordinate.Latitude, cityToSave.Coordinate.Longitude, date, cityToSave.CityId);
        }
        
        _logger.LogInformation("Solar data was found in the database.");
        
        foreach (var existingSolarData in existingSolarDataList)
        {
            // var existingCity = await _dbContext.Cities
            //     .Include(c => c.Coordinate)
            //     .FirstOrDefaultAsync(city => city.CityId == existingSolarData.CityId);
            _logger.LogInformation("Existing city is {cityName}", cityInDb?.Name ?? "nothing");

            // 3 - existing solar data in db but no city OR city exists but not for the current solar data - get city AND solar data from API, SAVE both
            if (cityInDb == null || existingSolarData.CityId != cityInDb.CityId)
            {
                _logger.LogInformation("No city was found in the database or solar data cityId was not for the city.");
                var cityToSave = await GetCityFromApi(cityName);
                return await GetSolarDataFromApi(cityToSave.Coordinate.Latitude, cityToSave.Coordinate.Longitude, date,
                    cityToSave.CityId);
            }

            _logger.LogInformation("Both solar data and city were found in the database.");
            // 4 - BOTH solar data and city exists - return the data from db
            if (existingSolarData.CityId == cityInDb.CityId)
            {
                return existingSolarData;
            }
        }

        return null;
    }

    private async Task<SolarData?> GetSolarDataFromApi(float lat, float lon, DateTime date, Guid cityId)
    {
        var solarData = await _solarDataProvider.GetSolarDataAsync(lat, lon, date);
        var solarDataToSave = _solarJsonProcessor.Process(solarData, date);
        _logger.LogInformation("Solar data is got from the API.");

        if (solarDataToSave != null)
        {
            solarDataToSave.CityId = cityId;
            _logger.LogInformation("CityId: {cityId} is saved to solar data", cityId);
            await _dbContext.SolarData.AddAsync(solarDataToSave);
            _logger.LogInformation("Solar data is added to the database");
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("The changes have been saved into the database.");
        }
        return solarDataToSave;
    }

    private async Task<City> GetCityFromApi(string cityName)
    {
        var cityData = await _cityDataProvider.GetCityDataAsync(cityName);
        var cityToSave = _cityJsonProcessor.Process(cityData);
        _logger.LogInformation("City is got from the API.");
        
        await _dbContext.Cities.AddAsync(cityToSave);
        _logger.LogInformation("City data is added to the database");
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("The changes have been saved into the database.");
        
        return cityToSave;
    }
}