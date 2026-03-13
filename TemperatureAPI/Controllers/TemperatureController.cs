using Microsoft.AspNetCore.Mvc;
using TemperatureAPI.DTO;
using TemperatureAPI.Interfaces;

namespace TemperatureAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemperatureController : ControllerBase
{
    private readonly ITemperatureService _temperatureService;
    private readonly ILogger<TemperatureController> _logger;

    public TemperatureController(ITemperatureService temperatureService, ILogger<TemperatureController> logger)
    {
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

        return Ok(result);
    }
}