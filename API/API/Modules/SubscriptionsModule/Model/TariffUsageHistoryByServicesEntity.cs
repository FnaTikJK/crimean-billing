using API.Modules.ServicesModule.Model;

namespace API.Modules.SubscriptionsModule.Model;

public class TariffUsageHistoryByServicesEntity
{
    public Guid TariffUsageHistoryId { get; set; }
    public TariffUsageHistoryEntity TariffUsageHistory { get; set; }
    public Guid ServiceTemplateId { get; set; }
    public ServiceTemplateEntity ServiceTemplate { get; set; }
    
    public float Spent { get; set; }
}