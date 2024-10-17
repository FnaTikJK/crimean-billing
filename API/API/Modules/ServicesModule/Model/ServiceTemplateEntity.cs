using System.ComponentModel.DataAnnotations;
using API.DAL;
using API.Modules.AccountsModule.Share;

namespace API.Modules.ServicesModule.Model;

public class ServiceTemplateEntity : IEntity
{
    [Key]
    public Guid Id { get; set; }
    public HashSet<ServiceEntity>? Services { get; set; }
    public required string Name { get; set; }
    public required string Code { get; set; }
    public required string Description { get; set; }
    public AccountType AccountType { get; set; }
    public ServiceType ServiceType { get; set; }
    public UnitType UnitType { get; set; }
    public bool IsTariffService { get; set; }
}