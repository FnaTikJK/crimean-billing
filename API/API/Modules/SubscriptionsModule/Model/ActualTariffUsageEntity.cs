using API.Modules.ServicesModule.Model;

namespace API.Modules.SubscriptionsModule.Model;

public class ActualTariffUsageEntity
{
    public Guid SubscriptionId { get; set; }
    public SubscriptionEntity Subscription { get; set; }
    public Guid ServiceTemplateId { get; set; }
    public ServiceTemplateEntity ServiceTemplate { get; set; }
    
    public float Spent { get; set; }
}