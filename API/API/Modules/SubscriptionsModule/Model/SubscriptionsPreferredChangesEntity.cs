using API.DAL;
using API.Modules.TariffsModule.Models;

namespace API.Modules.SubscriptionsModule.Model;

public class SubscriptionsPreferredChangesEntity
{
    public Guid SubscriptionId { get; set; }
    public SubscriptionEntity Subscription { get; set; }
    public Guid TariffTemplateId { get; set; }
    public TariffTemplateEntity TariffTemplate { get; set; }
}