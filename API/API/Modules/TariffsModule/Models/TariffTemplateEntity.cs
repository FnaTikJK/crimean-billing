using System.ComponentModel.DataAnnotations;
using API.DAL;
using API.Modules.AccountsModule.Share;

namespace API.Modules.TariffsModule.Models;

public class TariffTemplateEntity : IEntity
{
    [Key]
    public Guid Id { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required AccountType AccountType { get; set; }
    public required HashSet<TariffEntity> Tariffs { get; set; }
}