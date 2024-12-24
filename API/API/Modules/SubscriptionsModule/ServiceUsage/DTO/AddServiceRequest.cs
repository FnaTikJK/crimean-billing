namespace API.Modules.SubscriptionsModule.ServiceUsage.DTO;

public class AddServiceRequest
{
    public Guid AccountId { get; set; }
    public Guid ServiceId { get; set; }
}