using TemperatureAPI.Interfaces;
using TemperatureAPI.Models;

namespace TemperatureAPI.Service;

public class WeatherApiClient : IWeatherApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WeatherApiClient> _logger;

    public WeatherApiClient(HttpClient httpClient, ILogger<WeatherApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<WeatherApiResponse?> GetTemperatureAsync(int cityId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching temperature from WeatherAPI for cityId {CityId}", cityId);

        var response = await _httpClient.GetAsync($"/{cityId}", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("WeatherAPI returned {StatusCode} for cityId {CityId}", (int)response.StatusCode, cityId);
            return null;
        }

        var result = await response.Content.ReadFromJsonAsync<WeatherApiResponse>(cancellationToken: cancellationToken);
        _logger.LogInformation("WeatherAPI returned temperature {Temperature}°C for cityId {CityId}", result?.TemperatureC, cityId);
        return result;
    }
}