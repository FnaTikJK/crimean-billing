using API.Infrastructure.Extensions;
using API.Modules.AccountsModule.Share;
using API.Modules.UsersController.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Modules.UsersController;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUsersService usersService;

    public UsersController(IUsersService usersService)
    {
        this.usersService = usersService;
    }

    /// <summary>
    /// Фулл инфа о акке авторизованного пользователя.
    /// </summary>
    /// <remarks>
    /// Authorized на User
    /// </remarks>
    [Authorize(Roles = nameof(AccountRole.User))]
    [HttpGet("My")]
    public Task<ActionResult<UserDTO>> GetAuthUser() => GetUser(User.GetId());

    /// <summary>
    /// Получить инфу о пользователе. Для менеджеров
    /// </summary>
    /// <remarks>
    /// Authorized на Manager.
    /// </remarks>
    [Authorize(Roles = nameof(AccountRole.Manager))]
    [HttpGet("{userId:Guid}")]
    public async Task<ActionResult<UserDTO>> GetUser([FromRoute] Guid userId)
    {
        var response = await usersService.GetUser(userId);
        return response.ActionResult;
    }
}