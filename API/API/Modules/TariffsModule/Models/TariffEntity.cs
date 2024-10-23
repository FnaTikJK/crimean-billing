using System.ComponentModel.DataAnnotations;
using API.DAL;
using API.Modules.InvoiceModule.Model;
using API.Modules.SubscriptionsModule.Model;

namespace API.Modules.TariffsModule.Models;

public class TariffEntity : IEntity
{
    [Key]
    public Guid Id { get; set; }
    public TariffTemplateEntity Template { get; set; }
    public float Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public HashSet<TariffServiceAmountEntity> ServicesAmounts { get; set; }
    public HashSet<SubscriptionEntity>? Subscriptions { get; set; }
    public HashSet<InvoiceEntity>? Invoices { get; set; }
}