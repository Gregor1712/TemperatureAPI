using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TemperatureAPI.Interfaces;
using TemperatureAPI.Models;
using TemperatureAPI.Service;

namespace TemperatureAPI.Tests;

public class TemperatureServiceTests
{
    private readonly Mock<IWeatherApiClient> _weatherApiClientMock;
    private readonly TemperatureService _service;

    public TemperatureServiceTests()
    {
        _weatherApiClientMock = new Mock<IWeatherApiClient>();
        var loggerMock = new Mock<ILogger<TemperatureService>>();
        _service = new TemperatureService(_weatherApiClientMock.Object, loggerMock.Object);
    }

    [Fact]
    public async Task GetTemperatureAsync_ValidCity_ReturnsTemperature()
    {
        var apiResponse = new WeatherApiResponse(25.456m, DateTime.UtcNow);
        _weatherApiClientMock
            .Setup(x => x.GetTemperatureAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        var result = await _service.GetTemperatureAsync("bratislava");

        result.Should().NotBeNull();
        result!.City.Should().Be("Bratislava");
        result.TemperatureC.Should().Be(25.5m);
    }

    [Fact]
    public async Task GetTemperatureAsync_UnknownCity_ReturnsNull()
    {
        var result = await _service.GetTemperatureAsync("london");

        result.Should().BeNull();
        _weatherApiClientMock.Verify(
            x => x.GetTemperatureAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetTemperatureAsync_CityNameFormatting_FirstLetterUppercase()
    {
        var apiResponse = new WeatherApiResponse(10m, DateTime.UtcNow);
        _weatherApiClientMock
            .Setup(x => x.GetTemperatureAsync(2, It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        var result = await _service.GetTemperatureAsync("PRAHA");

        result.Should().NotBeNull();
        result!.City.Should().Be("Praha");
    }

    [Fact]
    public async Task GetTemperatureAsync_RoundsToOneDecimalPlace()
    {
        var apiResponse = new WeatherApiResponse(15.678m, DateTime.UtcNow);
        _weatherApiClientMock
            .Setup(x => x.GetTemperatureAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        var result = await _service.GetTemperatureAsync("bratislava");

        result.Should().NotBeNull();
        result!.TemperatureC.Should().Be(15.7m);
    }

    [Fact]
    public async Task GetTemperatureAsync_ApiReturnsNull_ReturnsCachedValue()
    {
        // First call succeeds and caches
        var apiResponse = new WeatherApiResponse(20m, DateTime.UtcNow);
        _weatherApiClientMock
            .Setup(x => x.GetTemperatureAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        await _service.GetTemperatureAsync("bratislava");

        // Second call - API returns null
        _weatherApiClientMock
            .Setup(x => x.GetTemperatureAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync((WeatherApiResponse?)null);

        var result = await _service.GetTemperatureAsync("bratislava");

        result.Should().NotBeNull();
        result!.TemperatureC.Should().Be(20m);
    }

    [Fact]
    public async Task GetTemperatureAsync_ApiThrowsException_ReturnsCachedValue()
    {
        // First call succeeds and caches
        var apiResponse = new WeatherApiResponse(18m, DateTime.UtcNow);
        _weatherApiClientMock
            .Setup(x => x.GetTemperatureAsync(3, It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        await _service.GetTemperatureAsync("budapest");

        // Second call - API throws
        _weatherApiClientMock
            .Setup(x => x.GetTemperatureAsync(3, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("Connection refused"));

        var result = await _service.GetTemperatureAsync("budapest");

        result.Should().NotBeNull();
        result!.TemperatureC.Should().Be(18m);
    }

    [Fact]
    public async Task GetTemperatureAsync_ApiFailsNoCache_ReturnsNull()
    {
        _weatherApiClientMock
            .Setup(x => x.GetTemperatureAsync(4, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("Connection refused"));

        var result = await _service.GetTemperatureAsync("vieden");

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTemperatureAsync_PreservesMeasuredAtUtc()
    {
        var measuredAt = new DateTime(2026, 3, 14, 9, 0, 0, DateTimeKind.Utc);
        var apiResponse = new WeatherApiResponse(22m, measuredAt);
        _weatherApiClientMock
            .Setup(x => x.GetTemperatureAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        var result = await _service.GetTemperatureAsync("bratislava");

        result.Should().NotBeNull();
        result!.MeasuredAtUtc.Should().Be(measuredAt);
    }
}