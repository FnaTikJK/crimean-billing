using API.Modules.ServicesModule.Model;

namespace API.Modules.SubscriptionsModule.ServiceUsage.Model.DTO;

public class ServiceUsageDTO
{
    public required Guid Id { get; set; }
    public required DateTime SubscribedAt { get; set; }
    public required float Spent { get; set; }
    public required float? Amount { get; set; }
    public required Guid ServiceId { get; set; }
    public required string ServiceName { get; set; }
    public required UnitType ServiceUnitType { get; set; }
}