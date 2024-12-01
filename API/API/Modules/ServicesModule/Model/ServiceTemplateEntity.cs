using System.ComponentModel.DataAnnotations;
using API.DAL;
using API.Modules.AccountsModule.Share;
using API.Modules.SubscriptionsModule.Model;
using API.Modules.TariffsModule.Models;

namespace API.Modules.ServicesModule.Model;

public class ServiceTemplateEntity : IEntity
{
    [Key]
    public Guid Id { get; set; }
    public HashSet<ServiceEntity>? Services { get; set; }
    public required string Name { get; set; }
    public required string Code { get; set; }
    public required string Description { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
    public AccountType AccountType { get; set; }
    public ServiceType ServiceType { get; set; }
    public UnitType UnitType { get; set; }
    public bool IsTariffService { get; set; }
    public HashSet<TariffServiceAmountEntity> AmountsInTariffs { get; set; }
    public HashSet<ActualTariffUsageEntity> ActualTariffUsages { get; set; }
    public HashSet<TariffUsageHistoryByServicesEntity> TariffUsageHistoryByServices { get; set; }
}