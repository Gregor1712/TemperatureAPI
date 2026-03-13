var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var cities = new Dictionary<int, string>
{
    [1] = "bratislava",
    [2] = "praha",
    [3] = "budapest",
    [4] = "vieden"
};

int GetTimeSlotSeed(DateTime utcNow)
{
    var date = utcNow.Date;
    var dayKey = date.Year * 366 + date.DayOfYear;
    if (utcNow.Hour >= 16)
        return dayKey * 2 + 1;
    if (utcNow.Hour >= 9)
        return dayKey * 2;
    // Before 9:00 — use previous day's 16:00 slot
    return (dayKey - 1) * 2 + 1;
}

app.MapGet("/{cityId:int}", (int cityId) =>
{
    if (!cities.ContainsKey(cityId))
        return Results.NotFound();

    var seed = GetTimeSlotSeed(DateTime.UtcNow) * 100 + cityId;
    var random = new Random(seed);
    var temperature = Math.Round((decimal)(random.NextDouble() * 40 - 10), 1);

    return Results.Ok(new
    {
        temperatureC = temperature,
        measuredAtUtc = DateTime.UtcNow
    });
});

app.Run();