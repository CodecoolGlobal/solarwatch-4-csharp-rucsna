using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Data;
using SolarWatch.Model;
using SolarWatch.Service.Repository;

namespace SolarWatch.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class CityController : ControllerBase
{
    private readonly ILogger<SolarWatchController> _logger;
    private readonly ICityService _service;

    public CityController(ILogger<SolarWatchController> logger, ICityService service)
    {
        _logger = logger;
        _service = service;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("ByName")]
    public async Task<ActionResult<IEnumerable<City>>> GetByNameAsync([Required] string cityName)
    {
        try
        {
            var result = await _service.GetCityByNameAsync(cityName);
            if (result != null)
            {
                return Ok(result);
            }

            return NotFound("No city was found with this name.");
        }
        catch (Exception e)
        {
            _logger.LogError(e,"Error getting city.");
            return BadRequest("Error getting city.");
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("All")]
    public async Task<ActionResult<IEnumerable<City>>> GetAllAsync()
    {
        try
        {
            var result = await _service.GetAllCitiesAsync();
            if (result != null)
            {
                return Ok(result);
            }

            return NotFound("No cities were found");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting cities");
            return BadRequest("Error getting cities");
        }
    }
    
    
    [HttpPost("NewCity")]
    public async Task<ActionResult<City>> AddCityAsync([Required] City newCity)
    {
        try
        {
            var result = await _service.CreateCityAsync(newCity.CityId, newCity.Name, newCity.Coordinate,
                newCity.Country, newCity.State == null ? newCity.State : string.Empty);
            if (result == null)
            {
                return Conflict($"City {newCity.Name} already exists in database");
            }
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error adding new city to database");
            return BadRequest("Error adding new city to database");
        }
    }
}