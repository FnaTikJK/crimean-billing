using API.Infrastructure;
using API.Modules.ServicesModule.Model;
using API.Modules.TariffsModule.DTO;
using API.Modules.TariffsModule.Extensions;
using API.Modules.TariffsModule.Models;
using API.Modules.TariffsModule.Models.DTO;

namespace API.Modules.TariffsModule;

public static class TariffsMapper
{
    public static TariffDTO Map(TariffTemplateEntity tariffTemplate)
    {
        var tariff = tariffTemplate.Tariffs.FindActual();
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

    public static void Map(
        TariffTemplateEntity targetTemplate, 
        PatchTariffRequest request,
        Dictionary<Guid, ServiceTemplateEntity> services)
    {
        if (request.Code != null)
            targetTemplate.Code = request.Code;
        if (request.Name != null)
            targetTemplate.Name = request.Name;
        if (request.Description != null)
            targetTemplate.Description = request.Description;
        if (request.AccountType != null)
            targetTemplate.AccountType = request.AccountType.Value;

        if (request.NeedActualizeTariff())
        {
            targetTemplate.Tariffs.FindActual().DeletedAt = DateTimeProvider.Now;
            targetTemplate.Tariffs.Add(MapTariff(request, services));
        }
    }

    private static TariffEntity MapTariff(PatchTariffRequest request, Dictionary<Guid, ServiceTemplateEntity> services)
        => MapTariff(request.Price!.Value, request.ServicesAmounts!, services);

    private static TariffEntity MapTariff(CreateTariffRequest request, Dictionary<Guid, ServiceTemplateEntity> services)
        => MapTariff(request.Price, request.ServicesAmounts, services);

    private static TariffEntity MapTariff(
        float price,
        IEnumerable<CreateTariffServiceAmountsRequest> serviceAmounts, 
        Dictionary<Guid, ServiceTemplateEntity> services)
    {
        var tariff = new TariffEntity()
        {
            Price = price,
            CreatedAt = DateTimeProvider.Now,
        };
        tariff.ServicesAmounts = serviceAmounts.Select(e => new TariffServiceAmountEntity()
        {
            Tariff = tariff,
            ServiceTemplate = services[e.ServiceTemplateId],
            Amount = e.Amount,
        }).ToHashSet();
        return tariff;
    }
}