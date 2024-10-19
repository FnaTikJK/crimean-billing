namespace API.Modules.TariffsModule.DTO;

public class CreateTariffServiceAmountsRequest
{
    public Guid ServiceTemplateId { get; set; }
    public float? Amount { get; set; }
}