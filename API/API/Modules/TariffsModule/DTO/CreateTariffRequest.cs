using API.Modules.AccountsModule.Share;

namespace API.Modules.TariffsModule.DTO;

public class CreateTariffRequest
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required AccountType AccountType { get; set; }
    
    public required float Price { get; set; }
    
    public IEnumerable<CreateTariffServiceAmountsRequest> ServicesAmounts { get; set; }
}

public class CreateTariffServiceAmountsRequest
{
    public Guid ServiceTemplateId { get; set; }
    public float? Amount { get; set; }
}