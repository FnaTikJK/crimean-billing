using API.DAL;
using API.Infrastructure;
using API.Modules.AccountsModule.Manager;
using API.Modules.ManagersController.DTO;
using API.Modules.ManagersController.Requests;
using Microsoft.EntityFrameworkCore;

namespace API.Modules.ManagersController;

public interface IManagersService
{
    Result<SearchManagersResponse> Search(SearchManagersRequest request);
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

    public Result<SearchManagersResponse> Search(SearchManagersRequest request)
    {
        var query = managers.AsNoTracking();

        if (request.Ids != null)
            query = query.Where(e => request.Ids.Contains(e.Id));
        if (request.Fio != null)
            query = query.Where(e => e.Fio.ToLower().Contains(request.Fio.ToLower()));

        var count = query.Count();
        var items = query.Skip(request.Skip).Take(request.Take).AsEnumerable().Select(ManagersMapper.Map);
        return Result.Ok(new SearchManagersResponse()
        {
            TotalCount = count,
            Items = items,
        });
    }

    public async Task<Result<ManagerDTO>> GetManager(Guid managerId)
    {
        var manager = await managers.AsNoTracking().FirstOrDefaultAsync(e => e.Id == managerId);
        if (manager == null)
            return Result.BadRequest<ManagerDTO>("Такого пользователя не существует");

        return Result.Ok(ManagersMapper.Map(manager));
    }
}