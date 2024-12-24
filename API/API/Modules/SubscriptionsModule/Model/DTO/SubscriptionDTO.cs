using API.Modules.SubscriptionsModule.ServiceUsage.Model.DTO;
using API.Modules.TariffsModule.Models.DTO;

namespace API.Modules.SubscriptionsModule.Model.DTO;

public class SubscriptionDTO
{
    public required Guid Id { get; set; }
    public required DateOnly PaymentDate { get; set; }
    public required TariffSubscriptionDTO Tariff { get; set; }
    public required IEnumerable<ServiceUsageDTO>? ServiceUsages { get; set; } 
    public required PreferredTariffDTO? PreferredTariff { get; set; }
    public required PreferredTariffDTO? ActualTariff { get; set; }
}