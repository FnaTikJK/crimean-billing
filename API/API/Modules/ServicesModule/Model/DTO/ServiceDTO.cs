using API.Modules.AccountsModule.Share;

namespace API.Modules.ServicesModule.Model.DTO;

public class ServiceDTO
{
    public Guid? Id { get; set; }
    public Guid TemplateId { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public bool IsTariffService { get; set; }
    public required string Description { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
    public AccountType AccountType { get; set; }
    public ServiceType ServiceType { get; set; }
    public UnitType UnitType { get; set; }
    
    public float? Price { get; set; }
    public float? Amount { get; set; }
}