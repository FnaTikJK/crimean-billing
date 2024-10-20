using API.Modules.TariffsModule.Models.DTO;

namespace API.Modules.SubscriptionsModule.Model.DTO;

public class SubscriptionDTO
{
    public Guid Id { get; set; }
    public DateOnly PaymentDate { get; set; }
    public TariffSubscriptionDTO Tariff { get; set; }
    public TariffSubscriptionDTO? PreferredTariff { get; set; }
}