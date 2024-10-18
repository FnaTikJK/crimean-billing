using API.Infrastructure;
using API.Modules.ServicesModule.Model;
using API.Modules.TariffsModule.DTO;
using API.Modules.TariffsModule.Models;
using API.Modules.TariffsModule.Models.DTO;

namespace API.Modules.TariffsModule;

public static class TariffsMapper
{
    public static TariffDTO Map(TariffTemplateEntity tariffTemplate)
    {
        var tariff = tariffTemplate.Tariffs.First();
        return new()
        {
            Id = tariff.Id,
            TemplateId = tariffTemplate.Id,
            Code = tariffTemplate.Code,
            Name = tariffTemplate.Name,
            Description = tariffTemplate.Description,
            AccountType = tariffTemplate.AccountType,
            Price = tariff.Price,
            Services = tariff.ServicesAmounts.Select(e => new ServiceAmountDTO()
            {
                TemplateId = e.Id,
                ServiceType = e.ServiceTemplate.ServiceType,
                UnitType = e.ServiceTemplate.UnitType,
                Amount = e.Amount,
            })
        };
    }
    
    public static TariffTemplateEntity Map(CreateTariffRequest request, Dictionary<Guid, ServiceTemplateEntity> services)
    {
        return new()
        {
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            AccountType = request.AccountType,
            Tariffs = new HashSet<TariffEntity>()
            {
                MapTariff(request, services)
            }
        };
    }

    private static TariffEntity MapTariff(CreateTariffRequest request, Dictionary<Guid, ServiceTemplateEntity> services)
    {
        var tariff = new TariffEntity()
        {
            Price = request.Price,
            CreatedAt = DateTimeProvider.Now,
        };
        tariff.ServicesAmounts = request.ServicesAmounts.Select(e => new TariffServiceAmountEntity()
        {
            Tariff = tariff,
            ServiceTemplate = services[e.ServiceTemplateId],
            Amount = e.Amount,
        }).ToHashSet();
        return tariff;
    }
}