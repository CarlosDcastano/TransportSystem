using Microsoft.EntityFrameworkCore;
using TransportSystem.Models;

namespace TransportSystem.Data;

public class TransportDbContext : DbContext
{
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<TransportService> TransportServices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        const string connectionString =
            "Server=204.168.220.65;Port=3306;Database=transport_system;User=root;Password=nebula123*;";

        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
}