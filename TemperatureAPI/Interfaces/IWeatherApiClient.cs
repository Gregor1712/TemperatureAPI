using TemperatureAPI.Models;

namespace TemperatureAPI.Interfaces;

public interface IWeatherApiClient
{
    Task<WeatherApiResponse?> GetTemperatureAsync(int cityId, CancellationToken cancellationToken = default);
}