using System.ComponentModel.DataAnnotations;
using API.DAL;

namespace API.Modules.TariffsModule.Models;

public class TariffEntity : IEntity
{
    [Key]
    public Guid Id { get; set; }
    public float Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public HashSet<TariffServiceAmountEntity> ServicesAmounts { get; set; }
}