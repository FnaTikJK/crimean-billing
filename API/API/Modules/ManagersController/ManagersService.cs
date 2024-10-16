using API.DAL;
using API.Infrastructure;
using API.Modules.AccountsModule.Manager;
using API.Modules.ManagersController.DTO;
using Microsoft.EntityFrameworkCore;

namespace API.Modules.ManagersController;

public interface IManagersService
{
    Task<Result<ManagerDTO>> GetManager(Guid managerId);
}

public class ManagersService : IManagersService
{
    private readonly DataContext db;
    private readonly ILog log;

    private readonly DbSet<ManagerEntity> managers;

    public ManagersService(DataContext db, ILog log)
    {
        this.db = db;
        this.log = log;
        this.managers = db.Managers;
    }

    public async Task<Result<ManagerDTO>> GetManager(Guid managerId)
    {
        var manager = await managers.AsNoTracking().FirstOrDefaultAsync(e => e.Id == managerId);
        if (manager == null)
            return Result.BadRequest<ManagerDTO>("Такого пользователя не существует");

        return Result.Ok(ManagersMapper.Map(manager));
    }
}