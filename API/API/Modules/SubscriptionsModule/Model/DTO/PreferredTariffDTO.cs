namespace API.Modules.TariffsModule.Models.DTO;

public class PreferredTariffDTO
{
    public required Guid TemplateId { get; set; }
    public required string Name { get; set; }
    public required float Price { get; set; }
    public required IEnumerable<ServiceAmountDTO> Services { get; set; }
}