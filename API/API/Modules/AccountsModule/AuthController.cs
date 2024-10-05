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
    
    [HttpPost("Managers/Register")]
    public async Task<ActionResult> Register([FromBody] RegisterManagerRequest request)
    {
        throw new NotImplementedException();

    }
    
    [HttpPost("Managers/Login")]
    public async Task<ActionResult> Register([FromBody] LoginManagerRequest request)
    {
        throw new NotImplementedException();
    }
    
    [HttpPost("Managers/ChangePassword")]
    public async Task<ActionResult> ChangePassword([FromBody] string request)
    {
        throw new NotImplementedException();
    }
    
   
    
    /// <summary>
    /// Регистрация для обычных юзеров
    /// </summary>
    [HttpPost("Register")]
    public async Task<ActionResult<RegisterUserResponse>> Register([FromBody] RegisterUserRequest request)
    {
        var response = await authService.Register(request);
        return response.ActionResult;
    }
    
    /// <summary>
    /// Вход для обычных юзеров. Необходимо ещё подтвердить вход
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
    /// Код из 6 цифр. Если хочешь скипнуть подтверждение - добавь телефон
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