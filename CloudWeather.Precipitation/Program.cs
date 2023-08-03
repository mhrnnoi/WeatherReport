using CloudWeather.Precipitation.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PrecipDbContext>(x =>
{
    x.EnableDetailedErrors();
    x.EnableSensitiveDataLogging();
    x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
},
     ServiceLifetime.Transient);


var app = builder.Build();
app.MapGet("/observation/{zip}", async (string zip, [FromQuery] int? days, PrecipDbContext db) =>
{
    if (days is null || days < 1 || days > 30)
    {
        return Results.BadRequest("plz enter days between 1 and 30 ");
    }
    var startDate = DateTime.UtcNow - TimeSpan.FromDays(days.Value);
    var result = await db.Precipitations.Where(x => x.ZipCode == zip && x.CreatedOn > startDate).ToListAsync();
    return Results.Ok(result);
});

app.MapPost("/observation", async (Precipitation precip, PrecipDbContext db) =>
{
    precip.CreatedOn = precip.CreatedOn.ToUniversalTime();
    await db.AddAsync(precip);
    await db.SaveChangesAsync();
});

app.Run();
