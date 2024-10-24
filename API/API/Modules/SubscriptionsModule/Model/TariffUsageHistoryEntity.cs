using System.ComponentModel.DataAnnotations;
using API.DAL;

namespace API.Modules.SubscriptionsModule.Model;

public class TariffUsageHistoryEntity : IEntity
{
    [Key]
    public Guid Id { get; set; }
    public SubscriptionEntity Subscription { get; set; }
    public HashSet<TariffUsageHistoryByServicesEntity> UsagesByServices { get; set; }
    public DateTime DateFrom {get; set; }
    public DateTime DateTo { get; set; }
}