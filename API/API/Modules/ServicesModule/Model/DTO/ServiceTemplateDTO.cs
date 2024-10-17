using API.Modules.AccountsModule.Share;

namespace API.Modules.ServicesModule.Model.DTO;

public class ServiceTemplateDTO
{
    public Guid Id { get; set; }
    public bool IsTariffService { get; set; }
    public required string Name { get; set; }
    public required string Code { get; set; }
    public required string Description { get; set; }
    public AccountType AccountType { get; set; }
    public ServiceType ServiceType { get; set; }
}