using API.DAL;
using API.Infrastructure;
using API.Modules.AccountsModule;
using API.Modules.AccountsModule.Manager.DTO;
using API.Modules.AccountsModule.Share;
using API.Modules.AdminModule;
using API.Modules.DatabaseModule.BaseEntities;
using API.Modules.DatabaseModule.RandomFillingData.Extensions;
using API.Modules.InvoiceModule;
using API.Modules.ServicesModule;
using API.Modules.ServicesModule.Model.DTO;
using API.Modules.SubscriptionsModule;
using API.Modules.SubscriptionsModule.DTO;
using API.Modules.TariffsModule;
using API.Modules.TariffsModule.Models.DTO;
using API.Modules.PaymentsModule;
using API.Modules.PaymentsModule.DTO;
using API.Modules.TariffsModule.DTO;

namespace API.Modules.DatabaseModule.RandomFillingData;

public interface IDatabaseServiceRandom
{
    Task<Result<bool>> RecreateRandomDatabase(Guid withAutoFilling);
}

public class DatabaseServiceRandomFilling : IDatabaseServiceRandom
{
    private readonly DataContext dataContext;
    private readonly IAuthService authService;
    private readonly IServicesService servicesService;
    private readonly ITariffsService tariffsService;
    private readonly ISubscriptionsService subscriptionsService;
    private readonly IPaymentsService paymentsService;
    private readonly IAdminService adminService;
    private readonly IInvoicesDaemon invoicesDaemon;
    private readonly IPaymentsDaemon paymentsDaemon;
    private static string codeService;
    private static Guid subId;
    private static Dictionary<string, ServiceDTO> service;

    public DatabaseServiceRandomFilling(
        DataContext dataContext,
        IAuthService authService,
        IServicesService servicesService,
        ITariffsService tariffsService,
        ISubscriptionsService subscriptionsService,
        IPaymentsService paymentsService,
        IAdminService invoicesDaemon, IInvoicesDaemon invoicesDaemon1, IPaymentsDaemon paymentsDaemon)
    {
        this.dataContext = dataContext;
        this.authService = authService;
        this.servicesService = servicesService;
        this.tariffsService = tariffsService;
        this.subscriptionsService = subscriptionsService;
        this.paymentsService = paymentsService;
        this.adminService = invoicesDaemon;
        this.invoicesDaemon = invoicesDaemon1;
        this.paymentsDaemon = paymentsDaemon;
    }

    public async Task<Result<bool>> RecreateRandomDatabase(Guid accountId)
    {
        var date = DateTimeProvider.Now;
        var addMoneyRequest = new AddMoneyRequest()
        {
            AccountId = accountId,
            ToAdd = 50000
        };

        await paymentsService.AddMoney(addMoneyRequest);
        var servicesByCode = await AddServices();
        service = servicesByCode;
        var tariffCode = await AddTariffs(servicesByCode);
        await AddSubscriptionsForAccount(accountId, tariffCode);

        for (int i = 0; i < 3; i++)
        {
            await subscriptionsService.SpendTariff(new()
            {
                SubscriptionId = subId,
                ServicesSpends = new List<SpendServiceTemplateRequest>
                {
                    new()
                    {
                        ServiceTemplateId = service[$"{codeService}"].TemplateId,
                        ToSpend = 1
                    }
                }
            });
            await CreateAndPayInvoice(date);
        }

        tariffCode = await AddTariffs(servicesByCode);
        await AddSubscriptionsForAccount(accountId, tariffCode);

        // for (int i = 0; i < 5; i++)
        // {
        //     await subscriptionsService.SpendTariff(new()
        //     {
        //         SubscriptionId = subId,
        //         ServicesSpends = new List<SpendServiceTemplateRequest>
        //         {
        //             new()
        //             {
        //                 ServiceTemplateId = service[$"{codeService}"].TemplateId,
        //                 ToSpend = 1
        //             }
        //         }
        //     });
        //     await CreateAndPayInvoice(date);
        // }
        //
        // tariffCode = await AddTariffs(servicesByCode);
        // await AddSubscriptionsForAccount(accountId, tariffCode);
        //
        // for (int i = 0; i < 5; i++)
        // {
        //     await subscriptionsService.SpendTariff(new()
        //     {
        //         SubscriptionId = subId,
        //         ServicesSpends = new List<SpendServiceTemplateRequest>
        //         {
        //             new()
        //             {
        //                 ServiceTemplateId = service[$"{codeService}"].TemplateId,
        //                 ToSpend = 1
        //             }
        //         }
        //     });
        //     await CreateAndPayInvoice(date);
        // }

        return Result.NoContent<bool>();
    }

    private async Task CreateAndPayInvoice(DateTime date)
    {
        adminService.MockDateTime(date.AddMonths(1));
        adminService.MockDateTime(date.AddDays(-3));
        await invoicesDaemon.CreateInvoices();
        adminService.MockDateTime(date.AddDays(3));
        await paymentsDaemon.TryPayInvoices();
    }

    private Task<Dictionary<string, ServiceDTO>> AddServices()
    {
        var servicesByCode = BaseServiceEntityRequests.CreateRandomServiceRequest
            .Select(async (e, i) =>
            {
                var response = await servicesService.CreateService(e);
                codeService = response.Value.Code;
                return response.IsSuccess
                    ? response.Value
                    : throw new Exception(response.Error);
            })
            .Select(e => e.Result)
            .ToDictionary(e => e.Code, e => e);

        Console.WriteLine(servicesByCode);
        return Task.FromResult(servicesByCode);
    }

    private Task<Dictionary<string, TariffDTO>> AddTariffs(Dictionary<string, ServiceDTO> servicesByCode)
    {
        var tariffsByCode = CreateRandomTariffSimRequest
            .Select(async createFunc =>
            {
                var ex = new DatabaseServiceRandomExtensions();
                var response =
                    await tariffsService.CreateTariff(createFunc(ex.GetRandomEnumValue<AccountType>(), servicesByCode));
                return response.IsSuccess
                    ? response.Value
                    : throw new Exception(response.Error);
            })
            .Select(e => e.Result)
            .ToDictionary(e => e.Code, e => e);

        return Task.FromResult(tariffsByCode);
    }

    private async Task AddSubscriptionsForAccount(Guid accountId,
        Dictionary<string, TariffDTO> tariffsByCode)
    {
        var sub = await subscriptionsService.Subscribe(new SubscribeRequest()
        {
            AccountId = accountId,
            TariffTemplateId = tariffsByCode.First().Value.TemplateId,
        });

        subId = sub.Value.Id;
    }

    private static Func<AccountType, Dictionary<string, ServiceDTO>, CreateTariffRequest>[]
        CreateRandomTariffSimRequest =
        {
            (accountType, s) => new CreateTariffRequest()
            {
                Code = "тариф созданный автоматикой номер: " + Guid.NewGuid(),
                Name = "Tariff Number = " + new Random().Next(0, 350),
                Description = "desc",
                Price = new Random().Next(0, 1000),
                AccountType = accountType,
                ServicesAmounts = new[]
                {
                    new CreateTariffServiceAmountsRequest()
                    {
                        Amount = 100,
                        ServiceTemplateId = s[$"{codeService}"].TemplateId,
                    }
                }
            }
        };

    // private SpendSubscriptionRequest spendSubscriptionRequest = new()
    // {
    //     SubscriptionId = subId,
    //     ServicesSpends = new List<SpendServiceTemplateRequest>
    //     {
    //         new()
    //         {
    //             ServiceTemplateId = service[$"{codeService}"].TemplateId,
    //             ToSpend = 1
    //         }
    //     }
    // };
}