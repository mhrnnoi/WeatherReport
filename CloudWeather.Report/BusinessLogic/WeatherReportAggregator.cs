using System.Text.Json;
using CloudWeather.Report.Config;
using CloudWeather.Report.DataAccess;
using CloudWeather.Report.Models;
using Microsoft.Extensions.Options;

namespace CloudWeather.Report.BusinessLogic;

public class WeatherReportAggregator : IWeatherReportAggregator
{
    private readonly IHttpClientFactory _http;
    private readonly ILogger<WeatherReportAggregator> _logger;
    private readonly WeatherDataConfig _weatherDataConfig;
    private readonly WeatherReportDbContext _db;

    public WeatherReportAggregator(IHttpClientFactory http, ILogger<WeatherReportAggregator> logger, IOptions<WeatherDataConfig> weatherDataConfig, WeatherReportDbContext db)
    {
        _http = http;
        _logger = logger;
        _weatherDataConfig = weatherDataConfig.Value;
        _db = db;
    }

    public async Task<WeatherReport> BuildReport(string zip, int days)
    {
        var httpClient = _http.CreateClient();
        var precipData = await FetchPrecipitationData(httpClient, zip, days);
        var totalSnow = GetTotalSnow(precipData);
        var totalRain = GetTotalRain(precipData);
        _logger.LogInformation
        (
         $"zip: {zip} over last {days} days: " +
         $"total snow: {totalSnow}, rain: {totalRain}"
         );
        var tempData = await FetchTemperatureData(httpClient, zip, days);
        var avarageHighTemp = tempData.Average(t => t.TempHigh);
        var avarageLowTemp = tempData.Average(t => t.TempLow);
        _logger.LogInformation
       (
        $"zip: {zip} over last {days} days: " +
        $"lo temp avg: {avarageLowTemp}, hi temp avg: {avarageHighTemp}"
        );

        var weatherReport = new WeatherReport
        {
            AvarageHigh = (int)avarageHighTemp,
            AvarageLow = (int)avarageLowTemp,
            RainfallTotalInches = (int)totalRain,
            SnowTotalInches = (int)totalSnow,
            ZipCode = zip,
            CreatedOn = DateTime.UtcNow

        };
        _db.Add(weatherReport);
        await _db.SaveChangesAsync();
        return weatherReport;
    }

    private static decimal GetTotalSnow(List<Precipitation> precipData)
    {
        var totalSnow = precipData
            .Where(p => p.WeatherType == "snow")
            .Sum(p => p.AmountInches);
        return Math.Round(Convert.ToDecimal(totalSnow), 1);
    }

    private static decimal GetTotalRain(List<Precipitation> precipData)
    {
        var totalRain = precipData
           .Where(p => p.WeatherType == "rain")
           .Sum(p => p.AmountInches);
        return Math.Round(Convert.ToDecimal(totalRain), 1);
    }

    private async Task<List<Temperature>> FetchTemperatureData(HttpClient httpClient, string zip, int days)
    {
        var endpoint = BuildTemperatureServiceEndpoint(zip, days);
        var temperatureRecords = await httpClient.GetAsync(endpoint);
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var temperatureData = await temperatureRecords
            .Content
            .ReadFromJsonAsync<List<Temperature>>(jsonSerializerOptions);
        return temperatureData ?? new List<Temperature>();
    }



    private async Task<List<Precipitation>> FetchPrecipitationData(HttpClient httpClient, string zip, int days)
    {
        var endpoint = BuildPrecipitationServiceEndpoint(zip, days);
        var precipRecords = await httpClient.GetAsync(endpoint);
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var precipData = await precipRecords
            .Content
            .ReadFromJsonAsync<List<Precipitation>>(jsonSerializerOptions);
        return precipData ?? new List<Precipitation>();
    }

    private string? BuildTemperatureServiceEndpoint(string zip, int days)
    {
        var tempServiceProtocol = _weatherDataConfig.TempDataProtocol;
        var tempServiceHost = _weatherDataConfig.TempDataHost;
        var tempServicePort = _weatherDataConfig.TempDataPort;
        return $"{tempServiceProtocol}://{tempServiceHost}:{tempServicePort}/observation/{zip}/days={days}";
    }
    private string? BuildPrecipitationServiceEndpoint(string zip, int days)
    {
        var precipServiceProtocol = _weatherDataConfig.PrecipDataProtocol;
        var precipServiceHost = _weatherDataConfig.PrecipDataHost;
        var precipServicePort = _weatherDataConfig.PrecipDataPort;
        return $"{precipServiceProtocol}://{precipServiceHost}:{precipServicePort}/observation/{zip}/days={days}";
    }
}
