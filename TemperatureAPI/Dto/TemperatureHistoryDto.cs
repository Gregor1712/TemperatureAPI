using TemperatureAPI.Entities;

namespace TemperatureAPI.DTO;

public class TemperatureHistoryDto : BaseEntity
{
    public required string City { get; set; }
    public decimal TemperatureC { get; set; }
    public DateTime MeasuredAtUtc { get; set; }
}