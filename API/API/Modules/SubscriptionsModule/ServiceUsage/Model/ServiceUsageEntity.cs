using System.ComponentModel.DataAnnotations;
using API.DAL;
using API.Modules.InvoiceModule.Model;
using API.Modules.ServicesModule.Model;
using API.Modules.SubscriptionsModule.Model;

namespace API.Modules.SubscriptionsModule.ServiceUsage.Model;

public class ServiceUsageEntity : IEntity
{
    [Key]
    public Guid Id { get; set; }
    
    public InvoiceEntity? Invoice { get; set; }
    public Guid ServiceId { get; set; }
    public required ServiceEntity Service { get; set; }
    public SubscriptionEntity? Subscription { get; set; }
    
    public required DateTime SubscribedAt { get; set; }
    public required float Spent { get; set; }
}