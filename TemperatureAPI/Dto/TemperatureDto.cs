namespace TemperatureAPI.DTO;

public class TemperatureDto
{
    public TemperatureDto(string city, decimal temperature, DateTime measuredAt)
    {
        City = city;
        TemperatureC = temperature;
        MeasuredAtUtc = measuredAt;
    }
    
    public string City { get; set; }
    public decimal TemperatureC { get; set; }
    public DateTime MeasuredAtUtc { get; set; }
}