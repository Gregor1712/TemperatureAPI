namespace TemperatureAPI.Models;

public static class CityMapping
{
    private static readonly Dictionary<string, int> Cities = new(StringComparer.OrdinalIgnoreCase)
    {
        ["bratislava"] = 1,
        ["praha"] = 2,
        ["budapest"] = 3,
        ["vieden"] = 4
    };

    public static bool TryGetCityId(string city, out int cityId) => Cities.TryGetValue(city, out cityId);

    public static IEnumerable<string> SupportedCities => Cities.Keys;
}