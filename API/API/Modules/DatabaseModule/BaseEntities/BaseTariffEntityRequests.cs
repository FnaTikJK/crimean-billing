using API.Modules.ServicesModule.Model.DTO;
using API.Modules.TariffsModule.DTO;

namespace API.Modules.DatabaseModule.BaseEntities;

public static class BaseTariffEntityRequests
{
    public static Func<Dictionary<string, ServiceDTO>, CreateTariffRequest>[] CreateRequests =
    {
        (s) => new CreateTariffRequest()
        {
            Code = "default",
            Name = "123name",
            Description = "desc",
            Price = 300,
            AccountType = 0,
            ServicesAmounts = new[]
            {
                new CreateTariffServiceAmountsRequest()
                {
                    Amount = 100,
                    ServiceTemplateId = s["withManyEdits"].TemplateId,
                }
            }
        }
    };
}