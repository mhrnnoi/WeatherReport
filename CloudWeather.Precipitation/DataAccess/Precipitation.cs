namespace CloudWeather.Precipitation.DataAccess;

public class Precipitation
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public int AmountInches { get; set; }
    public string WeatherType { get; set; }
    public string ZipCode { get; set; }
}