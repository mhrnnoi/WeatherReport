using CloudWeather.Temperature.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TemperatureDbContext>(x =>
{
    x.EnableDetailedErrors();
    x.EnableSensitiveDataLogging();
    x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
},
     ServiceLifetime.Transient);
var app = builder.Build();

app.MapGet("/observation/{zip}", async (string zip, [FromQuery] int? days, TemperatureDbContext db) =>
{
    if (days is null || days < 1 || days > 30)
    {
        return Results.BadRequest("plz enter days between 1 and 30 ");
    }
    var startDate = DateTime.UtcNow - TimeSpan.FromDays(days.Value);
    var result = await db.Temperatures.Where(x => x.ZipCode == zip && x.CreatedOn > startDate).ToListAsync();
    return Results.Ok(result);
});

app.MapPost("/observation", async (Temperature temperature, TemperatureDbContext db) =>
{
    temperature.CreatedOn = temperature.CreatedOn.ToUniversalTime();
    await db.AddAsync(temperature);
    await db.SaveChangesAsync();
});

app.Run();
