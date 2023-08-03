using Microsoft.EntityFrameworkCore;

namespace CloudWeather.Temperature.DataAccess;

public class TemperatureDbContext : DbContext
{
    protected TemperatureDbContext()
    {
    }
    public TemperatureDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Temperature> Temperatures { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }


}