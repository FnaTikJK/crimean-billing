using API.DAL;
using API.Infrastructure;
using API.Modules.AccountsModule;
using API.Modules.AccountsModule.Manager;
using API.Modules.AccountsModule.Manager.DTO;
using API.Modules.AccountsModule.Share;
using API.Modules.AccountsModule.User;
using API.Modules.AccountsModule.User.DTO;
using API.Modules.ServicesModule;
using API.Modules.ServicesModule.DTO;
using API.Modules.ServicesModule.Model;

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

    public DatabaseService(
        DataContext dataContext,
        IAuthService authService,
        IServicesService servicesService)
    {
        this.dataContext = dataContext;
        this.authService = authService;
        this.servicesService = servicesService;
    }
    
    public async Task<Result<bool>> RecreateDatabase(bool withAutoFilling)
    {
        dataContext.RecreateDatabase();

        if (withAutoFilling)
        {
            var managerIds = await AddManagers();
            var users = await AddUsers();
            await AddServices();
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

    private async Task<List<(Guid userId, List<Guid> accountIds)>> AddUsers()
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

        return new List<(Guid userId, List<Guid> accountIds)>()
        {
            (user.Value.UserId, new List<Guid> {account.Value.AccountId})
        };
    }

    private async Task AddServices()
    {
        var serviceOnlyTemplate = await servicesService.CreateService(new CreateServiceRequest()
        {
            Code = "542131",
            AccountType = AccountType.Sim,
            Name = "Сервис только из Template",
            Description = "Описание.фвафыв.афыа",
            ServiceType = ServiceType.Internet,
            IsTariffService = true,
            UnitType = UnitType.Mb,
            
            Price = null,
            Amount = null,
        });
        
        var simpleService1 = await servicesService.CreateService(new CreateServiceRequest()
        {
            Code = "1",
            AccountType = AccountType.Sim,
            Name = "Обычный сервис 1",
            Description = "Рандомное описание сервиса 1",
            ServiceType = ServiceType.Calls,
            IsTariffService = true,
            Price = 100,
            Amount = 100,
            UnitType = UnitType.Units,
        });

        var simpleService2 = await servicesService.CreateService(new CreateServiceRequest()
        {
            Code = "2",
            AccountType = AccountType.Sim,
            Name = "Обычный сервис 2",
            Description = "Так называемое описание",
            ServiceType = ServiceType.SMS,
            IsTariffService = false,
            Price = 81.5f,
            Amount = 33.4f,
            UnitType = UnitType.Units,
        });

        var simpleService3 = await servicesService.CreateService(new CreateServiceRequest()
        {
            Code = "3",
            AccountType = AccountType.Sim,
            Name = "Обычный сервис 3",
            Description = "Так называемое описание",
            ServiceType = ServiceType.MMS,
            IsTariffService = true,
            Price = 3453,
            Amount = null,
            UnitType = UnitType.Units,
        });
        
        var simpleService4 = await servicesService.CreateService(new CreateServiceRequest()
        {
            Code = "4",
            AccountType = AccountType.Sim,
            Name = "Обычный сервис 4",
            Description = "Описал сервис 4",
            ServiceType = ServiceType.Internet,
            IsTariffService = true,
            Price = 999,
            Amount = 234,
            UnitType = UnitType.Gb,
        });

        var serviceWithHistory = await servicesService.CreateService(new CreateServiceRequest()
        {
            Code = "11",
            AccountType = AccountType.Sim,
            Name = "У этого сервиса много редактирований(история)",
            Description = "Много изменений",
            ServiceType = ServiceType.Internet,
            IsTariffService = true,
            Price = 500,
            Amount = null,
            UnitType = UnitType.Mb,
        });

        await servicesService.PatchService(new PatchServiceRequest()
        {
            TemplateId = serviceWithHistory.Value.TemplateId,
            Price = 550,
            Amount = 9,
        });
        await servicesService.PatchService(new PatchServiceRequest()
        {
            TemplateId = serviceWithHistory.Value.TemplateId,
            Price = 600,
            Amount = 99,
        });
        await servicesService.PatchService(new PatchServiceRequest()
        {
            TemplateId = serviceWithHistory.Value.TemplateId,
            Price = 650,
            Amount = 999,
        });
    }
}