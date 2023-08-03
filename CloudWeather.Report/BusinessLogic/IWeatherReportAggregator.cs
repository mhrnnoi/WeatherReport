using CloudWeather.Report.DataAccess;

namespace CloudWeather.Report.BusinessLogic;

public interface IWeatherReportAggregator
{
    Task<WeatherReport> BuildReport(string zip, int days);
}