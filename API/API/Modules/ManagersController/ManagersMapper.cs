using API.Modules.AccountsModule.Manager;
using API.Modules.ManagersController.DTO;

namespace API.Modules.ManagersController;

public static class ManagersMapper
{
    public static ManagerDTO Map(ManagerEntity source)
        => new()
        {
            Id = source.Id,
            Fio = source.Fio,
        };
}