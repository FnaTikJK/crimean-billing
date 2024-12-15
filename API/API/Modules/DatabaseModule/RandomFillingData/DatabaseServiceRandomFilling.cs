using API.DAL;
using API.Infrastructure;
using API.Modules.AccountsModule;
using API.Modules.AccountsModule.Manager.DTO;
using API.Modules.AccountsModule.Share;
using API.Modules.AccountsModule.User.DTO;
using API.Modules.DatabaseModule.BaseEntities;
using API.Modules.DatabaseModule.RandomFillingData.Extensions;
using API.Modules.ServicesModule;
using API.Modules.ServicesModule.Model.DTO;
using API.Modules.SubscriptionsModule;
using API.Modules.SubscriptionsModule.DTO;
using API.Modules.TariffsModule;
using API.Modules.TariffsModule.Models.DTO;

namespace API.Modules.DatabaseModule.RandomFillingData;

public interface IDatabaseServiceRandom
{
    Task<Result<bool>> RecreateRandomDatabase(bool withAutoFilling);
}

public class DatabaseServiceRandomFilling : IDatabaseServiceRandom
{
    private readonly DataContext dataContext;
    private readonly IAuthService authService;
    private readonly IServicesService servicesService;
    private readonly ITariffsService tariffsService;
    private readonly ISubscriptionsService subscriptionsService;

    public DatabaseServiceRandomFilling(
        DataContext dataContext,
        IAuthService authService,
        IServicesService servicesService,
        ITariffsService tariffsService,
        ISubscriptionsService subscriptionsService)
    {
        this.dataContext = dataContext;
        this.authService = authService;
        this.servicesService = servicesService;
        this.tariffsService = tariffsService;
        this.subscriptionsService = subscriptionsService;
    }
    
    public async Task<Result<bool>> RecreateRandomDatabase(bool withAutoFilling)
    {
        dataContext.RecreateDatabase();
        var ex = new DatabaseServiceRandomExtensions();
        var accountsByUser = new List<Dictionary<Guid, List<Guid>>>();
        var tariffsByCode = new List<Dictionary<string, TariffDTO>>();
        var servicesByCode = await AddServices();

        if (withAutoFilling)
        {
            var managerIds = await AddManagers();
            for (int i = 0; i < 10; i++)
            {
                var userAccounts = await AddUsers(
                    ex.GenerateEmail(),
                    ex.GenerateFullName(),
                    ex.GeneratePhoneNumber(),
                    ex.GenerateNumber(),
                    ex.GetRandomEnumValue<AccountType>()
                );
                accountsByUser.Add(userAccounts);
            }

            for (int i = 0; i < 10; i++)
            {
                var tariffIndex = i % 5;
                var tariffCode = await AddTariffs(servicesByCode);
                tariffsByCode.Add(tariffCode);
                await AddSubscriptions(accountsByUser[i], tariffsByCode[tariffIndex]);
            }
        }

        Console.WriteLine(accountsByUser[0].Values);
        return Result.NoContent<bool>();
    }

    private async Task<List<Guid>> AddManagers()
    {
        var  response = await authService.RegisterManager(new RegisterManagerRequest()
        {
            Login = "string",
            Password = "string",
            Fio = "Менеджер Заполнен ПриСозданииБД"
        });
        return new() {response.Value.UserId};
    }

    private async Task<Dictionary<Guid, List<Guid>>> AddUsers(string email, string fio, string phoneNumber, string number, AccountType accountType )
    {
        var user = await authService.RegisterUser(new RegisterUserRequest()
        {
            Email = email,
            Fio = fio
        });
        var account = await authService.RegisterAccount(new RegisterAccountRequest
        {
            UserId = user.Value.UserId,
            Number = number,
            AccountType = accountType,
            PhoneNumber = phoneNumber,
        });

        return new Dictionary<Guid, List<Guid>>()
        {
            {user.Value.UserId, new List<Guid> {account.Value.AccountId}}
        };
    }

    private async Task<Dictionary<string, ServiceDTO>> AddServices()
    {
        var servicesByCode = BaseServiceEntityRequests.CreateRequests
            .Select(async (e, i) =>
            {
                var response = await servicesService.CreateService(e);
                return response.IsSuccess
                    ? response.Value
                    : throw new Exception(response.Error);
            })
            .Select(e => e.Result)
            .ToDictionary(e => e.Code, e => e);

        foreach (var patchReqs in BaseServiceEntityRequests.PatchRequests)
        {
            var response = await servicesService.PatchService(patchReqs(servicesByCode));
            if (!response.IsSuccess)
                throw new Exception(response.Error);

            servicesByCode[response.Value.Code] = response.Value;
        }

        return servicesByCode;
    }

    private async Task<Dictionary<string, TariffDTO>> AddTariffs(Dictionary<string, ServiceDTO> servicesByCode)
    {
        var tariffsByCode = BaseTariffEntityRequests.CreateRandomTariffSimRequest
            .Select(async createFunc =>
            {
                var ex = new DatabaseServiceRandomExtensions();
                var response = await tariffsService.CreateTariff(createFunc(ex.GetRandomEnumValue<AccountType>(), servicesByCode));
                return response.IsSuccess
                    ? response.Value
                    : throw new Exception(response.Error);
            })
            .Select(e => e.Result)
            .ToDictionary(e => e.Code, e => e);

        return tariffsByCode;
    }

    private async Task AddSubscriptions(Dictionary<Guid, List<Guid>> accountsByUser, Dictionary<string, TariffDTO> tariffsByCode)
    {
        var sub = await subscriptionsService.Subscribe(new SubscribeRequest()
        {
            AccountId = accountsByUser.First().Value.First(),
            TariffTemplateId = tariffsByCode.First().Value.TemplateId,
        });
    }
}