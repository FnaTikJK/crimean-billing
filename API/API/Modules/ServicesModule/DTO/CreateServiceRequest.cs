using API.Modules.AccountsModule.Share;
using API.Modules.ServicesModule.Model;

namespace API.Modules.ServicesModule.DTO;

public class CreateServiceRequest
{
    public required string Name { get; set; }
    public required string Code { get; set; }
    public required string Description { get; set; }
    public required AccountType AccountType { get; set; }
    public required ServiceType ServiceType { get; set; }
    public required UnitType UnitType { get; set; }
    public required bool IsTariffService { get; set; }
    public required float? Price { get; set; }
    public required float? Amount { get; set; }
}