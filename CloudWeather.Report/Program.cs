using CloudWeather.Report.BusinessLogic;
using CloudWeather.Report.Config;
using CloudWeather.Report.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WeatherReportDbContext>(x =>
{
    x.EnableDetailedErrors();
    x.EnableSensitiveDataLogging();
    x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
},
     ServiceLifetime.Transient);

builder.Services.AddHttpClient();
builder.Services.AddTransient<IWeatherReportAggregator, WeatherReportAggregator>();
builder.Services.AddOptions();
builder.Services.Configure<WeatherDataConfig>(builder.Configuration.GetSection("WeatherDataConfig"));

var app = builder.Build();

app.MapGet("/weather-report/{zip}", async (string zip, [FromQuery] int? days, IWeatherReportAggregator weatherAgg) =>
{
    if (days is null || days > 30 || days < 1)
    {
        return Results.BadRequest("plz enter days between 1 to 30 ");
    }
    var report = await weatherAgg.BuildReport(zip, days.Value);
    return Results.Ok(report);


});
app.Run();
