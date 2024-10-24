namespace API.Modules.SubscriptionsModule.Model.DTO;

public class TariffSubscriptionDTO
{
    public required Guid TemplateId { get; set; }
    public required string Name { get; set; }
    public required float Price { get; set; }
    public required IEnumerable<ServiceAmountsWithSpendsDTO> Services { get; set; }
}