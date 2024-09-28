using API.Infrastructure;
using API.Infrastructure.Config;
using Microsoft.EntityFrameworkCore;

namespace API.DAL;

public class DataContext : DbContext
{
    private readonly IConfiguration config;

    public DataContext(DbContextOptions options, IConfiguration config) : base(options)
    {
        this.config = config;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        options.UseNpgsql(
            Config.DatabaseConnectionString,
            builder => { builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null); });
    }

    public void RecreateDatabase()
    {
        this.Database.EnsureDeleted();
        this.Database.EnsureCreated();
    }
}