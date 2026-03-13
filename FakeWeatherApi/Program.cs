var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var cities = new Dictionary<int, string>
{
    [1] = "bratislava",
    [2] = "praha",
    [3] = "budapest",
    [4] = "vieden"
};

// Temperature changes at 9:00 and 16:00 UTC.
// "morning" slot: 09:00–15:59 → returns "2026-03-13_morning"
// "evening" slot: 16:00–08:59 (next day) → returns "2026-03-13_evening"
string GetTimeSlot(DateTime utcNow)
{
    if (utcNow.Hour >= 9 && utcNow.Hour < 16)
        return $"{utcNow:yyyy-MM-dd}_morning";

    // Evening slot: use today's date if 16+, yesterday's if before 9
    var date = utcNow.Hour >= 16 ? utcNow.Date : utcNow.Date.AddDays(-1);
    return $"{date:yyyy-MM-dd}_evening";
}
    
app.MapGet("/{cityId:int}", (int cityId) =>
{
    if (!cities.ContainsKey(cityId))
        return Results.NotFound();

    var slot = GetTimeSlot(DateTime.UtcNow);
    var seed = (slot + cityId).GetHashCode();
    var random = new Random(seed);
    var temperature = Math.Round((decimal)(random.NextDouble() * 40 - 10), 1);

    return Results.Ok(new
    {
        temperatureC = temperature,
        measuredAtUtc = DateTime.UtcNow
    });
});

app.Run();