using System.Collections.Concurrent;
using TemperatureAPI.DTO;
using TemperatureAPI.Interfaces;
using TemperatureAPI.Models;

namespace TemperatureAPI.Service;

public class TemperatureService : ITemperatureService
{
    private readonly IWeatherApiClient _weatherApiClient;
    private readonly ILogger<TemperatureService> _logger;

    private static readonly ConcurrentDictionary<string, TemperatureDto> Cache = new(StringComparer.OrdinalIgnoreCase);

    public TemperatureService(IWeatherApiClient weatherApiClient, ILogger<TemperatureService> logger)
    {
        _weatherApiClient = weatherApiClient;
        _logger = logger;
    }

    public async Task<TemperatureDto?> GetTemperatureAsync(string city, CancellationToken cancellationToken = default)
    {
        if (!CityMapping.TryGetCityId(city, out var cityId))
        {
            _logger.LogWarning("Unknown city requested: {City}", city);
            return null;
        }

        try
        {
            var apiResponse = await _weatherApiClient.GetTemperatureAsync(cityId, cancellationToken);

            if (apiResponse is not null)
            {
                var temperature = Math.Round(apiResponse.TemperatureC, 1);
                var response = new TemperatureDto(char.ToUpperInvariant(city[0]) + city[1..].ToLowerInvariant(), temperature, apiResponse.MeasuredAtUtc);

                Cache[city] = response;
                _logger.LogInformation("Updated cache for {City}: {Temperature}°C", city, temperature);
                return response;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch temperature from WeatherAPI for {City}", city);
        }

        if (Cache.TryGetValue(city, out var fallback))
        {
            _logger.LogWarning("Returning cached temperature for {City} due to WeatherAPI failure", city);
            return fallback;
        }

        _logger.LogError("No cached temperature available for {City} and WeatherAPI is unavailable", city);
        return null;
    }
}