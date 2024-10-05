using API.Infrastructure;
using API.Infrastructure.Config;
using API.Modules.AccountsModule.Manager;
using API.Modules.AccountsModule.User;
using Microsoft.EntityFrameworkCore;

namespace API.DAL;

public class DataContext : DbContext
{
    private readonly IConfiguration configuration;

    public DataContext(DbContextOptions options, IConfiguration configuration) : base(options)
    {
        this.configuration = configuration;
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

    public DbSet<ManagerEntity> Managers => Set<ManagerEntity>();
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<AccountEntity> Accounts => Set<AccountEntity>();
}