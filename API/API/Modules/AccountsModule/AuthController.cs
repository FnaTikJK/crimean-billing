using System.Security.Claims;
using API.Infrastructure.Extensions;
using API.Modules.AccountsModule.Manager.DTO;
using API.Modules.AccountsModule.Share;
using API.Modules.AccountsModule.User.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Modules.AccountsModule;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService authService;

    public AuthController(IAuthService authService)
    {
        this.authService = authService;
    }
    
    /// <summary>
    /// Инфа об аккаунте. Роли: 0 - user, 1 - manager
    /// </summary>
    [Authorize]
    [HttpGet("My")]
    public ActionResult GetMyAcc()
    {
        return Ok(new
        {
            Id = User.GetId(),
            Role = User.GetRole(),
        });
    }    
    
    /// <summary>
    /// Тестовая ручка для реги менеджера
    /// </summary>
    [HttpPost("Managers/Register")]
    public async Task<ActionResult> RegisterManager([FromBody] RegisterManagerRequest request)
    {
        var response = await authService.RegisterManager(request);
        return response.ActionResult;
    }
    
    /// <summary>
    /// Логин для менеджера 
    /// </summary>
    [HttpPost("Managers/Login")]
    public async Task<ActionResult> LoginManager([FromBody] LoginManagerRequest request)
    {
        var response = await authService.LoginManager(request);
        if (!response.IsSuccess)
            return response.ActionResult;
        
        var (loginResponse, credentials) = response.Value;
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(credentials));

        return Ok(loginResponse);
    }
    
    /// <summary>
    /// Authorized.
    /// Смена пароля для менеджера. После смены автоматически разлогинивает.
    /// </summary>
    [Authorize(Roles = nameof(AccountRole.Manager))]
    [HttpPost("Managers/ChangePassword")]
    public async Task<ActionResult> ChangeManagerPassword([FromBody] ChangeManagerPasswordRequest request)
    {
        var managerId = User.GetId();
        var response = await authService.ChangeManagerPassword(managerId, request);
        if (response.IsSuccess)
            await Logout();

        return response.ActionResult;
    }
    
    /// <summary>
    /// Регистрация для обычных юзеров
    /// </summary>
    /// <remarks>
    /// Если указан UserId, то добавляем ему Account <br/>
    /// Иначе создаём нового UserId, создаём и привязываем ему Account <br/>
    /// AccountType - Какого типа акк создать (SIM, TV, Internet)
    /// </remarks>
    [HttpPost("Register")]
    public async Task<ActionResult<RegisterUserResponse>> Register([FromBody] RegisterUserRequest request)
    {
        var response = await authService.Register(request);
        return response.ActionResult;
    }
    
    /// <summary>
    /// Вход для обычных юзеров. Необходимо ещё подтвердить вход через /Verify
    /// </summary>
    [HttpPost("Login")]
    public async Task<ActionResult> Login([FromBody] LoginUserRequest request)
    {
        var response = await authService.Login(request);
        return response.ActionResult;
    }

    /// <summary>
    /// Подтверждение входа для юзеров
    /// </summary>
    /// <remarks>
    /// Код из 6 цифр.
    /// <br/> Чтобы получить код без Tg/email - ручка в Админском контроллеке
    /// </remarks>
    [HttpPost("Verify")]
    public async Task<ActionResult<VerifyUserResponse>> VerifyLogin([FromBody] VerifyUserRequest request)
    {
        var response = await authService.Verify(request);
        if (!response.IsSuccess)
            return response.ActionResult;
        
        var (verifyResponse, credentials) = response.Value;
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(credentials));

        return Ok(verifyResponse);
    }
    
    /// <summary>
    /// Сбросить сессию
    /// </summary>
    [Authorize]
    [HttpPost("Logout")]
    public async Task<ActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return NoContent();
    }
}