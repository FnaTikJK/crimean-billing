using API.Infrastructure;
using API.Modules.ServicesModule.DTO;
using API.Modules.ServicesModule.Model;
using API.Modules.ServicesModule.Model.DTO;

namespace API.Modules.ServicesModule;

public static class ServicesMapper
{
    public static ServiceTemplateEntity MapTemplate(CreateServiceRequest source)
    {
        var service = MapService(source);
        return new()
        {
            Name = source.Name,
            Code = source.Code,
            Description = source.Description,
            AccountType = source.AccountType,
            ServiceType = source.ServiceType,
            UnitType = source.UnitType,
            IsTariffService = source.IsTariffService,
            Services = service != null 
                ? new HashSet<ServiceEntity> {service} 
                : null,
        };
    }

    public static ServiceEntity? MapService(CreateServiceRequest source)
        => source.Price == null
            ? null
            : new()
            {
                Price = source.Price.Value,
                Amount = source.Amount,
                CreatedAt = DateTimeProvider.Now,
            };

    public static ServiceDTO Map(ServiceTemplateEntity template, ServiceEntity? actualService)
        => new()
        {
            Id = actualService?.Id,
            TemplateId = template.Id,
            Code = template.Code,
            Description = template.Description,
            Name = template.Name,
            ServiceType = template.ServiceType,
            AccountType = template.AccountType,
            UnitType = template.UnitType,
            IsTariffService = template.IsTariffService,
            Price = actualService?.Price,
            Amount = actualService?.Amount,
        };

    public static ServiceDTO Map(ServiceTemplateEntity template)
    {
        var actualService = template.Services?.FirstOrDefault(e => e.DeletedAt == null);
        return new()
        {
            Id = actualService?.Id,
            TemplateId = template.Id,
            Code = template.Code,
            Description = template.Description,
            Name = template.Name,
            ServiceType = template.ServiceType,
            AccountType = template.AccountType,
            UnitType = template.UnitType,
            IsTariffService = template.IsTariffService,
            Price = actualService?.Price,
            Amount = actualService?.Amount,
        };
    }

    public static void Patch(PatchServiceRequest request, ServiceTemplateEntity target)
    {
        if (request.Code != null)
            target.Code = request.Code;
        if (request.Description != null)
            target.Description = request.Description;
        if (request.Name != null)
            target.Name = request.Name;
        if (request.ServiceType != null)
            target.ServiceType = request.ServiceType.Value;
        if (request.AccountType != null)
            target.AccountType = request.AccountType.Value;
        if (request.UnitType != null)
            target.UnitType = request.UnitType.Value;
        if (request.IsTariffService != null)
            target.IsTariffService = request.IsTariffService.Value;
    }

    public static ServiceEntity MapService(PatchServiceRequest request, ServiceEntity? previousService)
        => new()
        {
            Price = request.Price ?? previousService?.Price ?? -1,
            Amount = request.Amount ?? previousService?.Amount,
            CreatedAt = DateTimeProvider.Now,
        };

    public static ServiceHistoryDTO Map(ServiceEntity service)
        => new()
        {
            Price = service.Price,
            Amount = service.Amount,
            CreatedAt = service.CreatedAt,
            DeletedAt = service.DeletedAt.Value,
        };
}