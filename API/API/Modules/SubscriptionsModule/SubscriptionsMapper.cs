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
            Tariff = Map(subscription.Tariff, subscription.ActualTariffUsage)!,
            PreferredTariff = Map(subscription.PreferredChange?.TariffTemplate),
        };
    }
    
    private static TariffSubscriptionDTO Map(
        TariffEntity tariff, 
        IEnumerable<ActualTariffUsageEntity>? spends)
    {
        return new()
        {
            TemplateId = tariff.Template.Id,
            Name = tariff.Template.Name,
            Price = tariff.Price,
            Services = tariff.ServicesAmounts.Select(e => new ServiceAmountsWithSpendsDTO()
            {
                TemplateId = e.ServiceTemplateId,
                Name = e.ServiceTemplate.Name,
                ServiceType = e.ServiceTemplate.ServiceType,
                UnitType = e.ServiceTemplate.UnitType,
                Amount = e.Amount,
                Spent = spends?.FirstOrDefault(s => s.ServiceTemplateId == e.ServiceTemplateId)?.Spent 
                        ?? 0
            })
        };
    }
    
    private static PreferredTariffDTO? Map(TariffTemplateEntity? preferredTariffTemplate)
    {
        if (preferredTariffTemplate == null)
            return null;

        var tariff = preferredTariffTemplate.Tariffs.FindActual();
        return new()
        {
            TemplateId = preferredTariffTemplate.Id,
            Name = preferredTariffTemplate.Name,
            Price = tariff.Price,
            Services = tariff.ServicesAmounts.Select(e => new ServiceAmountDTO()
            {
                TemplateId = e.ServiceTemplateId,
                Name = e.ServiceTemplate.Name,
                ServiceType = e.ServiceTemplate.ServiceType,
                UnitType = e.ServiceTemplate.UnitType,
                Amount = e.Amount
            })
        };
    }
}