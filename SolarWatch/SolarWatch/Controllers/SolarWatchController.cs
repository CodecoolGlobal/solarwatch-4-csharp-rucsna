using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Model;
using SolarWatch.Service.Repository;

namespace SolarWatch.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class SolarWatchController : ControllerBase
{
    private readonly ILogger<SolarWatchController> _logger;
    private readonly ISolarWatchRepository _repository;

    public SolarWatchController(ILogger<SolarWatchController> logger, ISolarWatchRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }
    
    [Authorize(Roles = "Admin, User")]
    [HttpGet("GetSunrise_Sunset")]
    public async Task<ActionResult<SolarData>> GetAsync(string cityName, DateTime date)
    {
        try
        {
            var result = await _repository.GetDataAndAddToDbAsync(cityName, date);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("No solar data could be found.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting solar data");
            return BadRequest("Error getting solar data");
        }
    }
}