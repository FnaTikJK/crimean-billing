using System.ComponentModel.DataAnnotations;
using API.DAL;
using API.Modules.ServicesModule.Model;

namespace API.Modules.TariffsModule.Models;

public class TariffServiceAmountEntity : IEntity
{
    [Key]
    public Guid Id { get; set; }
    
    public required TariffEntity Tariff { get; set; }
    public required ServiceTemplateEntity ServiceTemplate { get; set; }
    public float? Amount { get; set; }
}