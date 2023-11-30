using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch.Controllers;
using SolarWatch.Model;
using SolarWatch.Processors;
using SolarWatch.Service;

namespace SolarWatchTest;

[TestFixture]
public class SolarWatchControllerTest
{
    private Mock<ILogger<SolarWatchController>>? _loggerMock;
    private Mock<ICityDataProvider>? _cityDataProviderMock;
    private Mock<ICityJsonProcessor>? _cityJsonProcessorMock;
    private Mock<ISolarDataProvider>? _solarDataProviderMock;
    private Mock<ISolarJsonProcessor>? _solarJsonProcessorMock;
    private SolarWatchController _controller;
    
    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<SolarWatchController>>();
        _cityDataProviderMock = new Mock<ICityDataProvider>();
        _cityJsonProcessorMock = new Mock<ICityJsonProcessor>();
        _solarDataProviderMock = new Mock<ISolarDataProvider>();
        _solarJsonProcessorMock = new Mock<ISolarJsonProcessor>();
        _controller = new SolarWatchController(_loggerMock.Object, _cityDataProviderMock.Object,
            _solarDataProviderMock.Object, _cityJsonProcessorMock.Object, _solarJsonProcessorMock.Object);
    }

    [Test]
    public async Task Test_GetReturnsNotFoundResult_IfCityDataProvider_Fails()
    {
        // Arrange
        _cityDataProviderMock?.Setup(x => x.GetCityDataAsync(It.IsAny<string>())).Throws(new Exception());
        
        // Act
        var result = await _controller.GetAsync("", DateTime.Now);
        
        // Assert
        Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
    }
    
    [Test]
    public async Task Test_GetReturnsNotFound_IfCityDataIsInvalid()
    {
        // Arrange
        var cityData = "{}";
        _cityDataProviderMock?.Setup(x => x.GetCityDataAsync(It.IsAny<string>())).ReturnsAsync(cityData);
        _cityJsonProcessorMock?.Setup(x => x.Process(cityData)).Throws<Exception>();
        
        // Act
        var result = await _controller.GetAsync("", DateTime.Now);
        
        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }
    
    // [Test]
    // public async Task Test_GetReturnsOkResult_IfCityDataIsValid()
    // {
    //     // Arrange
    //     var expectedSolarData = new SolarData();
    //     var solarData = "{}";
    //     var date = new DateTime(2023,11,11);
    //     _solarDataProviderMock?.Setup(x => x.GetSolarDataAsync(It.IsAny<float>(), It.IsAny<float>(), date)).ReturnsAsync(solarData);
    //     _solarJsonProcessorMock?.Setup(x => x.Process(solarData, "Paris", date)).Returns(expectedSolarData);
    //
    //     // Act
    //     var result = await _controller.GetAsync("Paris", date);
    //
    //     // Assert
    //     Assert.IsNotNull(result.Value);
    //     Assert.IsInstanceOf<ActionResult<SolarData>>(result.Value);
    //     Assert.That(result.Value, Is.EqualTo(expectedSolarData));
    // }
}