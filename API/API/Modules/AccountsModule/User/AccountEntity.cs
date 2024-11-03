using System.ComponentModel.DataAnnotations;
using API.DAL;
using API.Modules.AccountsModule.Share;
using API.Modules.InvoiceModule.Model;
using API.Modules.PaymentsModule.Model;
using API.Modules.SubscriptionsModule.Model;

namespace API.Modules.AccountsModule.User;

public class AccountEntity : IEntity
{
    [Key]
    public Guid Id { get; set; }
    public UserEntity User { get; set; }
    public SubscriptionEntity? Subscription { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Number { get; set; }
    public required float Money { get; set; }
    public required AccountType AccountType { get; set; }
    public HashSet<InvoiceEntity>? Invoices { get; set; }
    public HashSet<PaymentEntity>? Payments { get; set; }
}