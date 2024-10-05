using System.Security.Claims;
using System.Text.RegularExpressions;
using API.DAL;
using API.Infrastructure;
using API.Modules.AccountsModule.Manager;
using API.Modules.AccountsModule.Share;
using API.Modules.AccountsModule.User;
using API.Modules.AccountsModule.User.DTO;
using API.Modules.CacheModule;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace API.Modules.AccountsModule;

public interface IAuthService
{
    Task RegisterManager();
    Task LoginManager();
    Task<Result<RegisterUserResponse>> Register(RegisterUserRequest request);
    Task<Result<bool>> Login(LoginUserRequest request);
    Task<Result<(VerifyUserResponse, ClaimsIdentity)>> Verify(VerifyUserRequest request);
}

public class AuthService : IAuthService
{
    private readonly ICache cache;
    private readonly DataContext db;
    private readonly DbSet<UserEntity> users;
    private readonly DbSet<AccountEntity> accounts;
    private readonly DbSet<ManagerEntity> managers;
    private static Random rnd = new Random();
    private static Regex cacheRegex = new(@"[0-9]{6}");

    public AuthService(DataContext db, ICache cache)
    {
        this.cache = cache;
        this.db = db;
        users = db.Users;
        accounts = db.Accounts;
        managers = db.Managers;
    }

    public Task RegisterManager()
    {
        throw new NotImplementedException();
    }

    public Task LoginManager()
    {
        throw new NotImplementedException();
    }

    public async Task<Result<RegisterUserResponse>> Register(RegisterUserRequest request)
    {
        UserEntity? user;
        if (request.UserId != null)
        {
            user = await users.Include(e => e.Accounts).FirstOrDefaultAsync(e => e.Id == request.UserId);
            if (user == null)
                return Result.BadRequest<RegisterUserResponse>("Такого пользователя не сущесвтует");
        }
        else
        {
            user = new UserEntity();
            await users.AddAsync(user);
            user.Accounts = new HashSet<AccountEntity>();
        }

        var account = new AccountEntity
        {
            User = user, 
            PhoneNumber = request.PhoneNumber
        };
        user.Accounts.Add(account);
        await db.SaveChangesAsync();
        return Result.Ok(new RegisterUserResponse()
        {
            UserId = user.Id,
            AccountId = account.Id,
        });
    }
    
    public async Task<Result<bool>> Login(LoginUserRequest request)
    {
        var account = await accounts
            .AsNoTracking()
            .Include(e => e.User)
            .FirstOrDefaultAsync(e => e.PhoneNumber == request.PhoneNumber);
        if (account == null)
            return Result.BadRequest<bool>("Такого пользователя не существует");

        var verificationCode = string.Join("", Enumerable.Range(0, 6).Select(e => rnd.Next(10)));
        cache.Add(verificationCode, account.User.Id.ToString());
        return Result.NoContent<bool>();
    }

    public async Task<Result<(VerifyUserResponse, ClaimsIdentity)>> Verify(VerifyUserRequest request)
    {
        Guid userId;
        if (request.PhoneNumber != null)
        { // Скипаем верификацию (для тестирования)
            userId = (await accounts
                .AsNoTracking()
                .Include(e => e.User)
                .FirstAsync(e => e.PhoneNumber == request.PhoneNumber))
                .User.Id;
        }
        else
        {
            var cacheKey = request.verificationCode.ToString();
            if (!cacheRegex.IsMatch(cacheKey))
                return Result.BadRequest<(VerifyUserResponse, ClaimsIdentity)>(
                    "Некорректный формат кода. Regex: `[0-9]{6}`");
            if (!Guid.TryParse(cache.Get(cacheKey), out userId))
                return Result.BadRequest<(VerifyUserResponse, ClaimsIdentity)>("Некорректный код подтверждения");

            cache.Delete(cacheKey);
        }

        var user = await users
            .Include(e => e.Accounts)
            .AsNoTracking()
            .FirstAsync(e => e.Id == userId);
        var verifyResponse = new VerifyUserResponse
        {
            UserId = userId,
            AccountIds = user.Accounts.Select(e => e.Id).ToArray(),
        };
        return Result.Ok((verifyResponse, GetCredentials(user)));
    }


    private ClaimsIdentity GetCredentials(ManagerEntity manager)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, manager.Id.ToString()),
            new Claim(ClaimTypes.Role, nameof(AccountRole.Manager)),
        };
        return new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    }
    
    private ClaimsIdentity GetCredentials(UserEntity user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, nameof(AccountRole.User)),
        };
        return new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    }
}