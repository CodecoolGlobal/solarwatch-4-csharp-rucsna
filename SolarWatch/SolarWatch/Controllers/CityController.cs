using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Migrations.Users;
using SolarWatch.Model;
using SolarWatch.Service.Repository;

namespace SolarWatch.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class CityController : ControllerBase
{
    private readonly ILogger<SolarWatchController> _logger;
    private readonly ICityRepository _repository;
    private readonly IConfiguration _configuration;

    public CityController(ILogger<SolarWatchController> logger, ICityRepository service, IConfiguration configuration)
    {
        _logger = logger;
        _repository = service;
        _configuration = configuration;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("ByName")]
    public async Task<ActionResult<IEnumerable<City>>> GetByNameAsync([Required] string cityName)
    {
        try
        {
            var result = await _repository.GetCityByNameAsync(cityName);
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
            var result = await _repository.GetAllCitiesAsync();
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
    
    [Authorize(Roles = "Admin")]
    [HttpPost("NewCity")]
    public async Task<ActionResult<City>> AddCityAsync([Required] City newCity)
    {
        try
        {
            var result = await _repository.CreateCityAsync(newCity);
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

    [Authorize(Roles = "Admin")]
    [HttpPut("{cityId:guid}")]
    public async Task<IActionResult> UpdateCity([Required] Guid cityId, [FromBody] City updateRequest)
    {
        try
        {
            await _repository.UpdateCityAsync(cityId, updateRequest);
            return Ok($"The city on id: {cityId} successfully updated to {updateRequest}");
        }
        catch (Exception e)
        {
            _logger.LogError("Error updating city");
            return BadRequest("Error updating city");
        }
    }
}