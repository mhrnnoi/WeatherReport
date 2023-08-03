using CloudWeather.Temperature.DataAccess;
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

app.Run();
