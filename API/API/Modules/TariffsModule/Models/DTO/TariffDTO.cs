using API.Modules.AccountsModule.Share;

namespace API.Modules.TariffsModule.Models.DTO;

public class TariffDTO
{
    public Guid Id { get; set; }
    public Guid TemplateId { get; set; }
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required AccountType AccountType { get; set; }
    
    public float Price { get; set; }
    
    public IEnumerable<ServiceAmountDTO> Services { get; set; }
}