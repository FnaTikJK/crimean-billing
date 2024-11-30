using API.DAL;
using API.Infrastructure;
using API.Modules.AccountsModule.User;
using API.Modules.UsersController.DTO;
using API.Modules.UsersController.Requests;
using Microsoft.EntityFrameworkCore;

namespace API.Modules.UsersController;

public interface IUsersService
{
    Result<SearchUsersResponse> Search(SearchUsersRequest request);
    Task<Result<UserDTO>> GetUser(Guid userId);
    Task<Result<UserDTO>> PatchUserInfo(Guid userId, PatchUserRequest request);
}

public class UsersService : IUsersService
{
    private readonly DataContext db;
    private readonly ILog log;
    
    private readonly DbSet<UserEntity> users;

    public UsersService(DataContext db, ILog log)
    {
        this.db = db;
        users = db.Users;
        this.log = log;
    }

    public Result<SearchUsersResponse> Search(SearchUsersRequest request)
    {
        var query = users
            .Include(e => e.Accounts)
            .AsNoTracking();

        if (request.Ids != null)
            query = query.Where(e => request.Ids.Contains(e.Id));
        if (request.Email != null)
            query = query.Where(e => e.Email.ToLower().Contains(request.Email));
        if (request.Fio != null)
            query = query.Where(e => e.Fio.ToLower().Contains(request.Fio));
        if (request.PhoneNumber != null)
            query = query.Where(e => e.Accounts.Any(a => a.PhoneNumber.Contains(request.PhoneNumber)));
        if (request.AccountIds != null)
            query = query.Where(e => e.Accounts.Any(a => request.AccountIds.Contains(a.Id)));

        var count = query.Count();
        var items = query.Skip(request.Skip).Take(request.Take).AsEnumerable().Select(UsersMapper.Map);
        return Result.Ok(new SearchUsersResponse
        {
            TotalCount = count,
            Items = items,
        });
    }

    public async Task<Result<UserDTO>> GetUser(Guid userId)
    {
        var user = await users.AsNoTracking().Include(e => e.Accounts).FirstOrDefaultAsync(e => e.Id == userId);
        if (user == null)
            return Result.BadRequest<UserDTO>("Такого пользователя не существует");

        return Result.Ok(UsersMapper.Map(user));
    }

    public async Task<Result<UserDTO>> PatchUserInfo(Guid userId, PatchUserRequest request)
    {
        var user = await users.Include(e => e.Accounts).FirstOrDefaultAsync(e => e.Id == userId);
        if (user == null)
            return Result.BadRequest<UserDTO>("Такого пользователя не существует");
        
        UsersMapper.Patch(request, user);
        await db.SaveChangesAsync();
        return Result.Ok(UsersMapper.Map(user));
    }
}