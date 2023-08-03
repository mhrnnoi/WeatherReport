using Microsoft.EntityFrameworkCore;

namespace CloudWeather.Report.DataAccess;

public class WeatherReportDbContext : DbContext
{
    protected WeatherReportDbContext()
    {
    }
    public WeatherReportDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<WeatherReport> WeatherReports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }


}