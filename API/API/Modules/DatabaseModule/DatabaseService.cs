using API.DAL;
using API.Infrastructure;
using API.Modules.AccountsModule;
using API.Modules.AccountsModule.Manager;
using API.Modules.AccountsModule.Manager.DTO;
using API.Modules.AccountsModule.Share;
using API.Modules.AccountsModule.User;
using API.Modules.AccountsModule.User.DTO;
using API.Modules.DatabaseModule.BaseEntities;
using API.Modules.ServicesModule;
using API.Modules.ServicesModule.DTO;
using API.Modules.ServicesModule.Model;
using API.Modules.ServicesModule.Model.DTO;
using API.Modules.SubscriptionsModule;
using API.Modules.SubscriptionsModule.DTO;
using API.Modules.TariffsModule;
using API.Modules.TariffsModule.Models.DTO;

namespace API.Modules.DatabaseModule;

public interface IDatabaseService
{
    Task<Result<bool>> RecreateDatabase(bool withAutoFilling);
}

public class DatabaseService : IDatabaseService
{
    private readonly DataContext dataContext;
    private readonly IAuthService authService;
    private readonly IServicesService servicesService;
    private readonly ITariffsService tariffsService;
    private readonly ISubscriptionsService subscriptionsService;

    public DatabaseService(
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
    
    public async Task<Result<bool>> RecreateDatabase(bool withAutoFilling)
    {
        dataContext.RecreateDatabase();

        if (withAutoFilling)
        {
            var managerIds = await AddManagers();
            var accountsByUser = await AddUsers();
            var servicesByCode = await AddServices();
            var tariffsByCode = await AddTariffs(servicesByCode);
            await AddSubscriptions(accountsByUser, tariffsByCode);
        }
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

    private async Task<Dictionary<Guid, List<Guid>>> AddUsers()
    {
        var user = await authService.RegisterUser(new RegisterUserRequest()
        {
            Email = "test@test.test",
            Fio = "Пользователь Заполнен ПриСозданииБД"
        });
        var account = await authService.RegisterAccount(new RegisterAccountRequest
        {
            UserId = user.Value.UserId,
            Number = 999,
            AccountType = AccountType.Sim,
            PhoneNumber = "88005553535",
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
        var tariffsByCode = BaseTariffEntityRequests.CreateRequests
            .Select(async createFunc =>
            {
                var response = await tariffsService.CreateTariff(createFunc(servicesByCode));
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