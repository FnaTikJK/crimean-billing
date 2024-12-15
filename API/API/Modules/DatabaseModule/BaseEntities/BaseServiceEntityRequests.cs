using API.Modules.AccountsModule.Share;
using API.Modules.ServicesModule.DTO;
using API.Modules.ServicesModule.Model;
using API.Modules.ServicesModule.Model.DTO;

namespace API.Modules.DatabaseModule.BaseEntities;

public static class BaseServiceEntityRequests
{
    public static CreateServiceRequest[] CreateRequests = {
        new CreateServiceRequest()
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
        },
        new CreateServiceRequest()
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
        },
        new CreateServiceRequest()
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
        },
        new CreateServiceRequest()
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
        },
        new CreateServiceRequest()
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
        },
        new CreateServiceRequest()
        {
            Code = "withManyEdits",
            AccountType = AccountType.Sim,
            Name = "У этого сервиса много редактирований(история)",
            Description = "Много изменений",
            ServiceType = ServiceType.Internet,
            IsTariffService = true,
            Price = 500,
            Amount = null,
            UnitType = UnitType.Mb,
        },
    };

    public static CreateServiceRequest[] CreateRandomServiceRequest =
    {
        new CreateServiceRequest()
        {
            Code = new Random().Next(0, 9999).ToString(),
            AccountType = AccountType.Sim,
            Name = "Сервис созданный автоматикой номер: " + new Random().Next(0, 9999),
            Description = "Описание",
            ServiceType = ServiceType.Internet,
            IsTariffService = true,
            UnitType = UnitType.Mb,

            Price = null,
            Amount = null,
        }
    };

    public static Func<Dictionary<string, ServiceDTO>, PatchServiceRequest>[] PatchRequests = {
        (serviceByCode) => new PatchServiceRequest()
            {
                TemplateId = serviceByCode["withManyEdits"].TemplateId,
                Price = 550,
                Amount = 9,
            },
        (serviceByCode) => new PatchServiceRequest()
        {
            TemplateId = serviceByCode["withManyEdits"].TemplateId,
            Price = 600,
            Amount = 99,
        },
        (serviceByCode) => new PatchServiceRequest()
        {
            TemplateId = serviceByCode["withManyEdits"].TemplateId,
            Price = 650,
            Amount = 999,
        },
    };
}