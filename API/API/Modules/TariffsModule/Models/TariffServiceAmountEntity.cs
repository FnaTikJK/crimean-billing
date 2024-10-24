using System.ComponentModel.DataAnnotations;
using API.DAL;
using API.Modules.ServicesModule.Model;

namespace API.Modules.TariffsModule.Models;

public class TariffServiceAmountEntity
{
    public Guid TariffId { get; set; }
    public required TariffEntity Tariff { get; set; }
    public Guid ServiceTemplateId { get; set; }
    public required ServiceTemplateEntity ServiceTemplate { get; set; }
    public float? Amount { get; set; }
}