using TemperatureAPI.Entities;

namespace TemperatureAPI.Models;

public class TemperatureHistory : BaseEntity
{
    public required string City { get; set; }
    public decimal TemperatureC { get; set; }
    public DateTime MeasuredAtUtc { get; set; }
}