using API.DAL;
using API.Infrastructure;
using API.Modules.AccountsModule.User;
using API.Modules.UsersController.DTO;
using API.Modules.UsersController.Requests;
using Microsoft.EntityFrameworkCore;

namespace API.Modules.UsersController;

public interface IUsersService
{
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