namespace API.Modules.SubscriptionsModule.DTO;

public class SpendSubscriptionRequest
{
    public required Guid SubscriptionId { get; set; }
    public required List<SpendServiceTemplateRequest> ServicesSpends { get; set; }
}

public class SpendServiceTemplateRequest
{
    public required Guid ServiceTemplateId { get; set; }
    public required float ToSpend { get; set; }
}