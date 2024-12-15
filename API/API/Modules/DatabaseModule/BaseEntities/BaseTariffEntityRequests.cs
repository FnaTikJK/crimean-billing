using API.Modules.AccountsModule.Share;
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

    public static Func<AccountType, Dictionary<string, ServiceDTO>, CreateTariffRequest>[] CreateRandomTariffSimRequest =
    {
        (accountType, s) => new CreateTariffRequest()
        {
            Code = "default" + Guid.NewGuid(),
            Name = "Tariff Number = " + new Random().Next(0, 5000),
            Description = "desc",
            Price = new Random().Next(0, 5000),
            AccountType = accountType,
            ServicesAmounts = new[]
            {
                new CreateTariffServiceAmountsRequest()
                {
                    Amount = new Random().Next(30, 100),
                    ServiceTemplateId = s["withManyEdits"].TemplateId,
                }
            }
        }
    };
}