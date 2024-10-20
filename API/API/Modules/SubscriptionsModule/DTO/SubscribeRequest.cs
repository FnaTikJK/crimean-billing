namespace API.Modules.SubscriptionsModule.DTO;

public class SubscribeRequest
{
    public Guid TariffTemplateId { get; set; }
    public Guid AccountId { get; set; }
}