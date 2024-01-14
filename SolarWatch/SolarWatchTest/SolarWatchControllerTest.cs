using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch.Controllers;
using SolarWatch.Model;
using SolarWatch.Service.Processors;
using SolarWatch.Service;
using SolarWatch.Service.Repository;


namespace SolarWatchTest;

[TestFixture]
public class SolarWatchControllerTest
{
    private Mock<ILogger<SolarWatchController>>? _loggerMock;
    private Mock<ICityDataProvider>? _cityDataProviderMock;
    private Mock<ICityJsonProcessor>? _cityJsonProcessorMock;
    private Mock<ISolarWatchRepository> _repositoryMock;
    private SolarWatchController _controller;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<SolarWatchController>>();
        _cityDataProviderMock = new Mock<ICityDataProvider>();
        _cityJsonProcessorMock = new Mock<ICityJsonProcessor>();
        _repositoryMock = new Mock<ISolarWatchRepository>();
        _controller = new SolarWatchController(_loggerMock.Object, _repositoryMock.Object);
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
    //     var date = new DateTime(2023,11,11);
    //     var expectedSolarData = new SolarData{Date = date};
    //     //var solarData = "{}";
    //     // _solarDataProviderMock?.Setup(x => x.GetSolarDataAsync(It.IsAny<float>(), It.IsAny<float>(), date)).ReturnsAsync(solarData);
    //     // _solarJsonProcessorMock?.Setup(x => x.Process(solarData, "Paris", date)).Returns(expectedSolarData);
    //
    //     _repositoryMock.Setup(x => x.GetDataAndAddToDbAsync(It.IsAny<string>(), date)).ReturnsAsync(expectedSolarData);
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