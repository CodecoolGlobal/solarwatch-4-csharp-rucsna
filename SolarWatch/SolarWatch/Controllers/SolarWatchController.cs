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
}