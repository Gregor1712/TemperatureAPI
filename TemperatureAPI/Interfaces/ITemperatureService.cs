using TemperatureAPI.DTO;

namespace TemperatureAPI.Interfaces;

public interface ITemperatureService
{
    Task<TemperatureDto?> GetTemperatureAsync(string city, CancellationToken cancellationToken = default);
}