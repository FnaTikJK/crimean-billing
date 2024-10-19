using System.ComponentModel;
using API.Modules.AccountsModule.Share;
using API.Modules.ServicesModule.Model;

namespace API.Modules.ServicesModule.DTO;

public class PatchServiceRequest
{
    public Guid TemplateId { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }
    public AccountType? AccountType { get; set; }
    public ServiceType? ServiceType { get; set; }
    public UnitType? UnitType { get; set; }
    public bool? IsTariffService { get; set; }

    public float? Price { get; set; }
    public float? Amount { get; set; }
    
    [DefaultValue(false)]
    public bool? NeedKillService { get; set; }
}