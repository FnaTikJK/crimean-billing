using API.Infrastructure.Config;
using API.Modules.AccountsModule.Manager;
using API.Modules.AccountsModule.User;
using API.Modules.InvoiceModule.Model;
using API.Modules.PaymentsModule.Model;
using API.Modules.ServicesModule.Model;
using API.Modules.SubscriptionsModule.Model;
using API.Modules.TariffsModule.Models;
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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<TariffServiceAmountEntity>()
            .HasKey(e => new {e.TariffId, e.ServiceTemplateId});
        builder.Entity<SubscriptionsPreferredChangesEntity>()
            .HasKey(e => new {e.SubscriptionId, e.TariffTemplateId});
        builder.Entity<ActualTariffUsageEntity>()
            .HasKey(e => new {e.SubscriptionId, e.ServiceTemplateId});
        builder.Entity<TariffUsageHistoryByServicesEntity>()
            .HasKey(e => new {e.TariffUsageHistoryId, e.ServiceTemplateId});

        builder.Entity<PaymentEntity>()
            .HasOne(e => e.Invoice)
            .WithOne(e => e.Payment)
            .HasForeignKey<InvoiceEntity>(e => e.PaymentId)
            .IsRequired(false);
        builder.Entity<InvoiceEntity>()
            .HasOne(e => e.Payment)
            .WithOne(e => e.Invoice)
            .HasForeignKey<PaymentEntity>(e => e.InvoiceId)
            .IsRequired(false);
    }

    public void RecreateDatabase()
    {
        this.Database.EnsureDeleted();
        this.Database.EnsureCreated();
    }

    public DbSet<ManagerEntity> Managers => Set<ManagerEntity>();
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<AccountEntity> Accounts => Set<AccountEntity>();
    public DbSet<ServiceTemplateEntity> ServiceTemplates => Set<ServiceTemplateEntity>();
    public DbSet<ServiceEntity> Services => Set<ServiceEntity>();
    public DbSet<TariffTemplateEntity> TariffTemplates => Set<TariffTemplateEntity>();
    public DbSet<TariffEntity> Tariffs => Set<TariffEntity>();
    public DbSet<TariffServiceAmountEntity> TariffServiceAmounts => Set<TariffServiceAmountEntity>();
    public DbSet<SubscriptionEntity> Subscriptions => Set<SubscriptionEntity>();
    public DbSet<SubscriptionsPreferredChangesEntity> SubscriptionsPreferredChanges => Set<SubscriptionsPreferredChangesEntity>();
    public DbSet<InvoiceEntity> Invoices => Set<InvoiceEntity>();
    public DbSet<ActualTariffUsageEntity> ActualTariffUsages => Set<ActualTariffUsageEntity>();
    public DbSet<TariffUsageHistoryEntity> TariffUsageHistories => Set<TariffUsageHistoryEntity>();
    public DbSet<TariffUsageHistoryByServicesEntity> TariffUsageHistoryByServices => Set<TariffUsageHistoryByServicesEntity>();
    public DbSet<PaymentEntity> Payments => Set<PaymentEntity>();
}