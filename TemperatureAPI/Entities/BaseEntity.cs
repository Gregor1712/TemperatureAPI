using CsvHelper.Configuration.Attributes;

namespace TemperatureAPI.Entities;

public class BaseEntity
{
    [Ignore]
    public int Id { get; set; }  
}