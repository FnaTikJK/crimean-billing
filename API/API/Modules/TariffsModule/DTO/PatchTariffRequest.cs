using API.Modules.AccountsModule.Share;

namespace API.Modules.TariffsModule.DTO;

public class PatchTariffRequest
{
    public Guid TemplateId {get;set;}
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public AccountType? AccountType { get; set; }
    
    public float? Price { get; set; }
    public HashSet<CreateTariffServiceAmountsRequest>? ServicesAmounts { get; set; }
}

public static class PatchTariffRequestExtensions
{
    public static bool NeedActualizeTariff(this PatchTariffRequest request)
        => request.Price != null
           || request.ServicesAmounts != null;

    public static bool IsIncorrect(this PatchTariffRequest request, out string errorMessage)
    {
        errorMessage = null;
        if (request.Price != null && request.ServicesAmounts == null)
            errorMessage = "Tariff не может не иметь услуг";
        if (request.ServicesAmounts != null && request.Price == null)
            errorMessage = "Tariff не может не иметь цену";

        return errorMessage != null;
    }
}