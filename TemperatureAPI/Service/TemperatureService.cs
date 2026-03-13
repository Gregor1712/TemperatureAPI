using System.Collections.Concurrent;
using TemperatureAPI.DTO;
using TemperatureAPI.Interfaces;
using TemperatureAPI.Models;

namespace TemperatureAPI.Service;

public class TemperatureService : ITemperatureService
{
    private readonly IWeatherApiClient _weatherApiClient;
    private readonly ILogger<TemperatureService> _logger;

    private static readonly ConcurrentDictionary<string, CachedTemperature> Cache = new(StringComparer.OrdinalIgnoreCase);

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

        var now = DateTime.UtcNow;
        var currentSlot = GetTimeSlot(now);

        if (Cache.TryGetValue(city, out var cached) && cached.TimeSlot == currentSlot)
        {
            _logger.LogDebug("Returning cached temperature for {City} (slot {TimeSlot})", city, currentSlot);
            return cached.Response;
        }

        try
        {
            var apiResponse = await _weatherApiClient.GetTemperatureAsync(cityId, cancellationToken);

            if (apiResponse is not null)
            {
                var temperature = Math.Round(apiResponse.TemperatureC, 2);
                var response = new TemperatureDto(city.ToLowerInvariant(), temperature, apiResponse.MeasuredAtUtc);

                Cache[city] = new CachedTemperature(response, currentSlot);
                _logger.LogInformation("Updated cache for {City}: {Temperature}°C (slot {TimeSlot})", city, temperature, currentSlot);
                return response;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch temperature from WeatherAPI for {City}", city);
        }

        // Return last known value if external API fails
        if (Cache.TryGetValue(city, out var fallback))
        {
            _logger.LogWarning("Returning stale cached temperature for {City} due to WeatherAPI failure", city);
            return fallback.Response;
        }

        _logger.LogError("No cached temperature available for {City} and WeatherAPI is unavailable", city);
        return null;
    }

    /// <summary>
    /// Temperature changes at 9:00 and 16:00 UTC, so we define time slots:
    /// Slot is based on date + which half of the day we're in (before 9, 9-16, after 16).
    /// </summary>
    private static string GetTimeSlot(DateTime utcNow)
    {
        var date = utcNow.Date;
        if (utcNow.Hour < 9)
            return $"{date:yyyy-MM-dd}_00";
        if (utcNow.Hour < 16)
            return $"{date:yyyy-MM-dd}_09";
        return $"{date:yyyy-MM-dd}_16";
    }

    private record CachedTemperature(TemperatureDto Response, string TimeSlot);
}