namespace API.Modules.SubscriptionsModule.ServiceUsage.DTO;

public class SpendServiceRequest
{
    public Guid AccountId { get; set; }
    public Guid ServiceId { get; set; }
    public float ToSpend { get; set; }
}