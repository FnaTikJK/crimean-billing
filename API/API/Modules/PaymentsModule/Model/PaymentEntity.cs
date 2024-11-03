using System.ComponentModel.DataAnnotations;
using API.DAL;
using API.Modules.AccountsModule.User;
using API.Modules.InvoiceModule.Model;

namespace API.Modules.PaymentsModule.Model;

public class PaymentEntity : IEntity
{
    [Key]
    public Guid Id { get; set; }
    public required float Money { get; set; }
    public required PaymentType Type { get; set; }
    public required DateTime DateTime { get; set; }
    
    public Guid AccountId { get; set; }
    public required AccountEntity Account { get; set; }
    public Guid? InvoiceId { get; set; }
    public InvoiceEntity? Invoice { get; set; }
}

public enum PaymentType
{
    Deposit,
    Withdrawal,
}