namespace TemperatureAPI.Specifications;

public class TemperatureHistorySpecParams: PagingParams
{
    private string? _city;
    public string? City
    {
        get => _city;
        set => _city = value?.ToLower();
    }
    
    private decimal? _temperatureC;
    public decimal? TemperatureC
    {
        get => _temperatureC;
        set => _temperatureC = value;
    }

    public string? Sort { get; set; }
    
    private string? _search;
    public string Search
    {
        get => _search ?? "";
        set => _search = value.ToLower();
    }
}