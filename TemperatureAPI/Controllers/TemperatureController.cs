using Microsoft.AspNetCore.Mvc;
using TemperatureAPI.DTO;
using TemperatureAPI.Interfaces;
using TemperatureAPI.Models;

namespace TemperatureAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemperatureController : BaseApiController
{
    private readonly ITemperatureService _temperatureService;
    private readonly ILogger<TemperatureController> _logger;
    private readonly IUnitOfWork _unit;

    public TemperatureController(IUnitOfWork unit, ITemperatureService temperatureService, ILogger<TemperatureController> logger)
    {
        _unit = unit;
        _temperatureService = temperatureService;
        _logger = logger;
    }

    [HttpGet("{city}")]
    public async Task<ActionResult<TemperatureDto>> GetTemperature(string city, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Temperature requested for city: {City}", city);

        var result = await _temperatureService.GetTemperatureAsync(city, cancellationToken);

        if (result is null)
        {
            _logger.LogWarning("Temperature not available for city: {City}", city);
            return NotFound(new { error = $"Temperature for city '{city}' is not available." });
        }

        await InsertHistory(result);

        return Ok(result);
    }

    private async Task InsertHistory(TemperatureDto result)
    {
        var history = new TemperatureHistory
        {
            City = result.City,
            TemperatureC = result.TemperatureC,
            MeasuredAtUtc = result.MeasuredAtUtc
        };
        _unit.Repository<TemperatureHistory>().Add(history);
        await _unit.Complete();
    }
}