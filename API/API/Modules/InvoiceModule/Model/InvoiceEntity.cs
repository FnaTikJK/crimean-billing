using System.ComponentModel.DataAnnotations;
using API.DAL;
using API.Modules.AccountsModule.User;
using API.Modules.TariffsModule.Models;

namespace API.Modules.InvoiceModule.Model;

public class InvoiceEntity : IEntity
{
    [Key]
    public Guid Id { get; set; }
    public AccountEntity Account { get; set; }
    public TariffEntity Tariff { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? PayedAt { get; set; }
}