using API.Modules.ServicesModule.Model;

namespace API.Modules.TariffsModule.Models.DTO;

public class ServiceAmountDTO
{
    public Guid TemplateId { get; set; }
    public ServiceType ServiceType { get; set; }
    public UnitType UnitType { get; set; }
    public float? Amount { get; set; }
}