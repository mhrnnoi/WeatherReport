namespace CloudWeather.Temperature.DataAccess;

public class Temperature
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public int TempHigh { get; set; }
    public int TempLow { get; set; }
    public string ZipCode { get; set; }
}