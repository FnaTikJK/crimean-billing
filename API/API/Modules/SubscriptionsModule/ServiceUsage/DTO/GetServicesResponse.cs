using API.Modules.SubscriptionsModule.ServiceUsage.Model.DTO;

namespace API.Modules.SubscriptionsModule.ServiceUsage.DTO;

public class GetServicesResponse
{
    public IEnumerable<ServiceUsageDTO> ServiceUsages { get; set; }
}