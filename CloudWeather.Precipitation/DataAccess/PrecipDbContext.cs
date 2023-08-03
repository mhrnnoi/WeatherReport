using Microsoft.EntityFrameworkCore;

namespace CloudWeather.Precipitation.DataAccess;

public class PrecipDbContext : DbContext
{
    protected PrecipDbContext()
    {
    }
    public PrecipDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Precipitation> Precipitations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }


}