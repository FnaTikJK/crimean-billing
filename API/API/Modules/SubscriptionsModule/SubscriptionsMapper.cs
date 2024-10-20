using API.Modules.SubscriptionsModule.Model;
using API.Modules.SubscriptionsModule.Model.DTO;
using API.Modules.TariffsModule.Extensions;
using API.Modules.TariffsModule.Models;
using API.Modules.TariffsModule.Models.DTO;

namespace API.Modules.SubscriptionsModule;

public static class SubscriptionsMapper
{
    public static SubscriptionDTO Map(SubscriptionEntity subscription)
    {
        return new()
        {
            Id = subscription.Id,
            PaymentDate = DateOnly.FromDateTime(subscription.PaymentDate),
            Tariff = Map(subscription.Tariff.Template, subscription.Tariff)!,
            PreferredTariff = Map(subscription.PreferredChange?.TariffTemplate, null),
        };
    }
    
    private static TariffSubscriptionDTO? Map(TariffTemplateEntity? tariffTemplate, TariffEntity? tariff)
    {
        if (tariffTemplate == null)
            return null;
        
        tariff ??= tariffTemplate.Tariffs.FindActual();
        return new()
        {
            TemplateId = tariffTemplate.Id,
            Name = tariffTemplate.Name,
            Price = tariff.Price,
            Services = tariff.ServicesAmounts.Select(e => new ServiceAmountDTO
            {
                TemplateId = e.Id,
                Name = e.ServiceTemplate.Name,
                ServiceType = e.ServiceTemplate.ServiceType,
                UnitType = e.ServiceTemplate.UnitType,
                Amount = e.Amount,
            })
        };
    }
}