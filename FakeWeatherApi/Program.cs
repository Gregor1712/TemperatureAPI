var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var temperatures = new Dictionary<int, string>
{
    [1] = "bratislava",
    [2] = "praha",
    [3] = "budapest",
    [4] = "vieden"
};

var random = new Random(42);

app.MapGet("/{cityId:int}", (int cityId) =>
{
    if (!temperatures.ContainsKey(cityId))
        return Results.NotFound();

    var temperature = Math.Round((decimal)(random.NextDouble() * 40 - 10), 2);

    return Results.Ok(new
    {
        temperatureC = temperature,
        measuredAtUtc = DateTime.UtcNow
    });
});

app.Run();