using Microsoft.AspNetCore.Mvc;
using SolarWatch.Model;
using SolarWatch.Processors;
using SolarWatch.Service;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class SolarWatchController : ControllerBase
{
    private readonly ILogger<SolarWatchController> _logger;
    private readonly ICityDataProvider _cityDataProvider;
    private readonly ISolarDataProvider _solarDataProvider;
    private readonly ICityJsonProcessor _cityJsonProcessor;
    private readonly ISolarJsonProcessor _solarJsonProcessor;

    public SolarWatchController(ILogger<SolarWatchController> logger, ICityDataProvider cityDataProvider, ISolarDataProvider solarDataProvider, ICityJsonProcessor cityJsonProcessor, ISolarJsonProcessor solarJsonProcessor)
    {
        _logger = logger;
        _cityDataProvider = cityDataProvider;
        _solarDataProvider = solarDataProvider;
        _cityJsonProcessor = cityJsonProcessor;
        _solarJsonProcessor = solarJsonProcessor;
    }
    
    [HttpGet("GetSunrise_Sunset")]
    public async Task<ActionResult<SolarData>> GetAsync(string cityName, DateTime date)
    {
        try
        {
            var cityData = await _cityDataProvider.GetCityDataAsync(cityName);
            
            var lat = _cityJsonProcessor.Process(cityData).Latitude;
            var lon = _cityJsonProcessor.Process(cityData).Longitude;

            var solarData = await _solarDataProvider.GetSolarDataAsync(lat, lon, date);

            return Ok(_solarJsonProcessor.Process(solarData, cityName, date));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting solar data");
            return NotFound("Error getting solar data");
        }
        
    }
}