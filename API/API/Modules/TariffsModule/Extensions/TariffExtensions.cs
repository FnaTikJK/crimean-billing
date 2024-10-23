using API.Modules.TariffsModule.Models;

namespace API.Modules.TariffsModule.Extensions;

public static class TariffExtensions
{
    public static TariffEntity FindActualTariff(this TariffTemplateEntity template)
        => FindActual(template.Tariffs);
    
    public static TariffEntity FindActual(this IEnumerable<TariffEntity> collection)
        => collection.First(e => e.DeletedAt == null);

    public static bool IsNotActual(this TariffEntity tariff) => tariff.DeletedAt != null;
}