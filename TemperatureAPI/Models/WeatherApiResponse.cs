namespace TemperatureAPI.Models;

public record WeatherApiResponse(decimal TemperatureC, DateTime MeasuredAtUtc);