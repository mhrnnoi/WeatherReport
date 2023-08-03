namespace CloudWeather.Report.DataAccess;

public class WeatherReport
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public int AvarageHigh { get; set; }
    public int AvarageLow { get; set; }
    public int RainfallTotalInches { get; set; }
    public int SnowTotalInches { get; set; }
    public string ZipCode { get; set; }
}