using System.ComponentModel.DataAnnotations;
using API.DAL;
using API.Modules.AccountsModule.User;
using API.Modules.SubscriptionsModule.ServiceUsage.Model;
using API.Modules.TariffsModule.Models;

namespace API.Modules.SubscriptionsModule.Model;

public class SubscriptionEntity : IEntity
{
    [Key]
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public AccountEntity Account { get; set; }
    public TariffEntity Tariff { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime PaymentDate { get; set; }
    
    public HashSet<ActualTariffUsageEntity>? ActualTariffUsage { get; set; }
    public HashSet<TariffUsageHistoryEntity>? UsageHistories { get; set; } 

    public SubscriptionsPreferredChangesEntity? PreferredChange { get; set; }

    public HashSet<ServiceUsageEntity>? ServiceUsages { get; set; }
}