using API.Infrastructure.Extensions;
using API.Modules.AccountsModule.Share;
using API.Modules.UsersController.DTO;
using API.Modules.UsersController.Requests;
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
    /// Патчинг юзера. Здесь можно привязать ТГ для увед
    /// </summary>
    [Authorize(Roles = nameof(AccountRole.User))]
    [HttpPatch("")]
    public async Task<ActionResult<UserDTO>> PatchUserInfo(PatchUserRequest request)
    {
        var response = await usersService.PatchUserInfo(User.GetId(), request);
        return response.ActionResult;
    }

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
    
    /// <summary>
    /// Поиск по пользователям. Для менеджеров
    /// </summary>
    /// <remarks>
    /// Authorized на Manager.
    /// </remarks>
    [Authorize(Roles = nameof(AccountRole.Manager))]
    [HttpPost("Search")]
    public ActionResult<SearchUsersResponse> Search([FromBody] SearchUsersRequest request)
    {
        var response = usersService.Search(request);
        return response.ActionResult;
    }
}