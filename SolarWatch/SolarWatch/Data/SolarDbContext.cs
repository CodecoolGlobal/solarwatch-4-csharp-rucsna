using Microsoft.EntityFrameworkCore;
using SolarWatch.Model;

namespace SolarWatch.Data;

public class SolarDbContext : DbContext
{
    public DbSet<City> Cities { get; set; }
    public DbSet<Coordinate> Coordinates { get; set; }
    public DbSet<SolarData> SolarData { get; set; }

    public SolarDbContext(DbContextOptions<SolarDbContext> options)
        : base(options)
    {
    }
}