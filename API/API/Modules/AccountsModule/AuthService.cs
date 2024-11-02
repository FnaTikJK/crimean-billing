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
    Task<Result<RegisterUserResponse>> RegisterUser(RegisterUserRequest request);
    Task<Result<RegisterAccountResponse>> RegisterAccount(RegisterAccountRequest request);
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

        var newManager = AuthMapper.Map(request, passwordHasher.Hash(request.Password));
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
        var manager = await managers.AsNoTracking()
            .FirstOrDefaultAsync(e => e.Login == request.Login);
        if (manager == null || !passwordHasher.VerifyPassword(request.Password, manager.PasswordHash))
            return Result.BadRequest<(LoginManagerResponse, ClaimsIdentity)>("Неправильный логин или пароль");

        return Result.Ok((
            new LoginManagerResponse { UserId = manager.Id },
            GetCredentials(manager)));
    }

    public async Task<Result<bool>> ChangeManagerPassword(Guid managerId, ChangeManagerPasswordRequest request)
    {
        var manager = await managers.FirstOrDefaultAsync(e => e.Id == managerId);
        if (manager == null)
            return Result.BadRequest<bool>("Такого менеджера не существует");

        if (!passwordHasher.VerifyPassword(request.OldPassword, manager.PasswordHash))
            return Result.BadRequest<bool>("Старый пароль не совпадает");

        manager.PasswordHash = passwordHasher.Hash(request.NewPassword);
        await db.SaveChangesAsync();

        log.Info($"Changed password for Manger: {managerId}");
        return Result.NoContent<bool>();
    }

    public async Task<Result<RegisterUserResponse>> RegisterUser(RegisterUserRequest request)
    {
        var userWithSameEmail = await users.AsNoTracking().FirstOrDefaultAsync(e => e.Email == request.Email);
        if (userWithSameEmail != null)
            return Result.BadRequest<RegisterUserResponse>("Email занят");

        var user = AuthMapper.Map(request);
        await users.AddAsync(user);
        await db.SaveChangesAsync();

        log.Info($"Create new User: {user.Id}");
        return Result.Ok(new RegisterUserResponse()
        {
            UserId = user.Id
        });
    }

    public async Task<Result<RegisterAccountResponse>> RegisterAccount(RegisterAccountRequest request)
    {
        var isNumberOccupied = await accounts.AsNoTracking().FirstOrDefaultAsync(e => e.Number == request.Number);
        if (isNumberOccupied != null)
            return Result.BadRequest<RegisterAccountResponse>("Лицевой счёт с таким номером уже существует");

        var user = await users.Include(e => e.Accounts).FirstOrDefaultAsync(e => e.Id == request.UserId);
        if (user == null)
            return Result.BadRequest<RegisterAccountResponse>("Такого пользователя не существует");
        var userWithSamePhone = (await accounts.AsNoTracking().Include(e => e.User)
                .FirstOrDefaultAsync(e => e.PhoneNumber == request.PhoneNumber))
            ?.User.Id;
        if (userWithSamePhone != null && userWithSamePhone != user.Id)
            return Result.BadRequest<RegisterAccountResponse>("Телефон уже привязан к другому пользователю");

        var account = AuthMapper.Map(request);
        user.Accounts.Add(account);
        await db.SaveChangesAsync();

        return Result.Ok(new RegisterAccountResponse
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
        cache.AddOrUpdate(verificationCode, userId);
        cache.AddOrUpdate(PhoneConverter.ToPhoneWithoutRegMask(request.PhoneNumber)!, verificationCode);
        notificationService.SendEmail("Код подтверждения", verificationCode, account.User.Email);
        return Result.NoContent<bool>();
    }

    public async Task<Result<(VerifyUserResponse, ClaimsIdentity)>> Verify(VerifyUserRequest request)
    {
        var cacheKey = request.VerificationCode;
        if (!cacheRegex.IsMatch(cacheKey))
            return Result.BadRequest<(VerifyUserResponse, ClaimsIdentity)>(
                "Некорректный формат кода. Regex: `[0-9]{6}`");
        if (!Guid.TryParse(cache.Get(cacheKey), out var userId))
            return Result.BadRequest<(VerifyUserResponse, ClaimsIdentity)>("Некорректный код подтверждения");

        var user = await users
            .Include(e => e.Accounts)
            .AsNoTracking()
            .FirstAsync(e => e.Id == userId);
        cache.Delete(cacheKey);
        foreach (var truncatedPhone in user.Accounts.Select(a => PhoneConverter.ToPhoneWithoutRegMask(a.PhoneNumber)))
            cache.Delete(truncatedPhone!);

        log.Info($"Verified User: {userId}");
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