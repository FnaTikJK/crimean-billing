using API.Modules.AccountsModule.User;

namespace API.Modules.SubscriptionsModule.Model;

public class RelationAccountSubscriptionEntity
{
    public AccountEntity Account { get; set; }
    public Guid AccountId { get; set; }
    public SubscriptionEntity Subscriptions { get; set; }
    public Guid SubscriptionId { get; set; }
}