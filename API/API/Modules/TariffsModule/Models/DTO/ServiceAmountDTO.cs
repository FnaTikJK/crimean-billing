using API.Modules.ServicesModule.Model;

namespace API.Modules.TariffsModule.Models.DTO;

public class ServiceAmountDTO
{
    public required Guid TemplateId { get; set; }
    public required string Name { get; set; }
    public required ServiceType ServiceType { get; set; }
    public required UnitType UnitType { get; set; }
    public required float? Amount { get; set; }
}