using API.DAL;
using API.Infrastructure;
using API.Modules.AccountsModule.User;
using API.Modules.UsersController.DTO;
using Microsoft.EntityFrameworkCore;

namespace API.Modules.UsersController;

public interface IUsersService
{
    Task<Result<UserDTO>> GetUser(Guid userId);
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
}