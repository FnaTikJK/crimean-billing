using API.Modules.SubscriptionsModule.ServiceUsage.Model;
using API.Modules.SubscriptionsModule.ServiceUsage.Model.DTO;

namespace API.Modules.SubscriptionsModule.ServiceUsage;

public static class ServiceUsageMapper
{
    public static ServiceUsageDTO Map(ServiceUsageEntity entity)
        => new()
        {
            Id = entity.Id,
            Spent = entity.Spent,
            SubscribedAt = entity.SubscribedAt,
            Amount = entity.Service.Amount,
            ServiceId = entity.ServiceId,
            ServiceName = entity.Service.Template.Name,
            Price = entity.Service.Price,
            ServiceType = entity.Service.Template.ServiceType,
            ServiceUnitType = entity.Service.Template.UnitType,
            ServiceTemplateId = entity.Service.Template.Id,
        };
}