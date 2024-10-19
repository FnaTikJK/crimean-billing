using API.Modules.TariffsModule.Models;

namespace API.Modules.TariffsModule.Extensions;

public static class TariffExtensions
{
    public static TariffEntity FindActual(this IEnumerable<TariffEntity> collection)
        => collection.First(e => e.DeletedAt == null);
}