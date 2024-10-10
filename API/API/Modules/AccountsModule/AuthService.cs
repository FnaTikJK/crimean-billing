using System.Security.Claims;
using System.Text.RegularExpressions;
using API.DAL;
using API.Infrastructure;
using API.Modules.AccountsModule.Manager;
using API.Modules.AccountsModule.Manager.DTO;
using API.Modules.AccountsModule.Share;
using API.Modules.AccountsModule.User;
using API.Modules.AccountsModule.User.DTO;
using API.Modules.CacheModule;
using API.Modules.NotificationModule;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace API.Modules.AccountsModule;

public interface IAuthService
{
    Task<Result<RegisterManagerResponse>> RegisterManager(RegisterManagerRequest request);
    Task<Result<(LoginManagerResponse, ClaimsIdentity)>> LoginManager(LoginManagerRequest request);
    Task<Result<bool>> ChangeManagerPassword(Guid managerId, ChangeManagerPasswordRequest request);
    Task<Result<RegisterUserResponse>> Register(RegisterUserRequest request);
    Task<Result<bool>> Login(LoginUserRequest request);
    Task<Result<(VerifyUserResponse, ClaimsIdentity)>> Verify(VerifyUserRequest request);
}

public class AuthService : IAuthService
{
    private readonly ICache cache;
    private readonly DataContext db;
    private readonly IPasswordHasher passwordHasher;
    private readonly ILog log;
    private readonly INotificationService notificationService;

    private readonly DbSet<UserEntity> users;
    private readonly DbSet<AccountEntity> accounts;
    private readonly DbSet<ManagerEntity> managers;
    private static Random rnd = new();
    private static Regex cacheRegex = new(@"[0-9]{6}");

    public AuthService(
        ICache cache,
        DataContext db, 
        IPasswordHasher passwordHasher, 
        ILog log,
        INotificationService notificationService)
    {
        this.cache = cache;
        this.db = db;
        this.passwordHasher = passwordHasher;
        this.log = log;
        this.notificationService = notificationService;
        
        users = db.Users;
        accounts = db.Accounts;
        managers = db.Managers;
    }

    public async Task<Result<RegisterManagerResponse>> RegisterManager(RegisterManagerRequest request)
    {
        var existed = await managers.AsNoTracking().FirstOrDefaultAsync(e => e.Login == request.Login);
        if (existed != null)
            return Result.BadRequest<RegisterManagerResponse>("Такой логин занят");

        var newManager = new ManagerEntity
        {
            Login = request.Login,
            PasswordHash = passwordHasher.Hash(request.Password)
        };
        await managers.AddAsync(newManager);
        await db.SaveChangesAsync();
        
        log.Info($"Registered Manager: {newManager.Id}");
        return Result.Ok(new RegisterManagerResponse
        {
            UserId = newManager.Id,
        });
    }

    public async Task<Result<(LoginManagerResponse, ClaimsIdentity)>> LoginManager(LoginManagerRequest request)
    {
        var hashed = passwordHasher.Hash(request.Password);
        var manager = await managers.AsNoTracking()
            .FirstOrDefaultAsync(e => e.Login == request.Login
                                      && e.PasswordHash == hashed);
        if (manager == null)
            return Result.BadRequest<(LoginManagerResponse, ClaimsIdentity)>("Неправильный логин или пароль");

        return Result.Ok((
            new LoginManagerResponse {UserId = manager.Id},
            GetCredentials(manager)));
    }

    public async Task<Result<bool>> ChangeManagerPassword(Guid managerId, ChangeManagerPasswordRequest request)
    {
        var manager = await managers.FirstOrDefaultAsync(e => e.Id == managerId);
        if (manager == null)
            return Result.BadRequest<bool>("Такого менеджера не существует");

        var hashed = passwordHasher.Hash(request.OldPassword);
        if (manager.PasswordHash != hashed)
            return Result.BadRequest<bool>("Старый пароль не совпадает");

        manager.PasswordHash = passwordHasher.Hash(request.NewPassword);
        await db.SaveChangesAsync();
        
        log.Info($"Changed password for Manger: {managerId}");
        return Result.NoContent<bool>();
    }

    public async Task<Result<RegisterUserResponse>> Register(RegisterUserRequest request)
    {
        var isNumberOccupied = await accounts.AsNoTracking().FirstOrDefaultAsync(e => e.Number == request.Number);
        if (isNumberOccupied != null)
            return Result.BadRequest<RegisterUserResponse>("Лицевой счёт с таким номером уже существует");
        var userWithSamePhone = (await accounts.AsNoTracking().Include(e => e.User)
            .FirstOrDefaultAsync(e => e.PhoneNumber == request.PhoneNumber))
            ?.User.Id;
        
        UserEntity? user;
        if (request.UserId != null)
        {
            user = await users.Include(e => e.Accounts).FirstOrDefaultAsync(e => e.Id == request.UserId);
            if (user == null)
                return Result.BadRequest<RegisterUserResponse>("Такого пользователя не существует");
            if (userWithSamePhone != null && userWithSamePhone != user.Id)
                return Result.BadRequest<RegisterUserResponse>("Телефон уже привязан к другому пользователю");
        }
        else
        {
            if (userWithSamePhone != null)
                return Result.BadRequest<RegisterUserResponse>("Телефон уже привязан к другому пользователю");
            if (string.IsNullOrEmpty(request.Email))
                return Result.BadRequest<RegisterUserResponse>("Невозможно создать пользователя без почты");
            log.Info($"Create new User with PhoneNumber: {request.PhoneNumber}.");
            user = UserMapper.Map(request);
            await users.AddAsync(user);
        }

        var account = AccountMapper.Map(request);
        user.Accounts.Add(account);
        await db.SaveChangesAsync();
        return Result.Ok(new RegisterUserResponse
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
        var userId = account.User.Id.ToString();
        log.Info($"Set verification code for User: {userId}, PhoneNumber: {request.PhoneNumber}, VerificationCode: {verificationCode}");
        cache.Add(verificationCode, userId);
        notificationService.SendEmail("Код подтверждения", verificationCode, account.User.Email);
        return Result.NoContent<bool>();
    }

    public async Task<Result<(VerifyUserResponse, ClaimsIdentity)>> Verify(VerifyUserRequest request)
    {
        Guid userId;
        if (request.PhoneNumber != null)
        { // Скипаем верификацию (для тестирования)
            log.Info($"Skip verification for PhoneNumber: {request.PhoneNumber}");
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
        
        log.Info($"Verified User: {userId}");
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