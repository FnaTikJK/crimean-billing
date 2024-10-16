using API.Infrastructure.Extensions;
using API.Modules.AccountsModule.Share;
using API.Modules.ManagersController.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Modules.ManagersController;

[Route("api/[controller]")]
[ApiController]
public class ManagersController : ControllerBase
{
    private readonly IManagersService managersService;

    public ManagersController(IManagersService managersService)
    {
        this.managersService = managersService;
    }

    [Authorize(Roles = nameof(AccountRole.Manager))]
    [HttpGet("My")]
    public async Task<ActionResult<ManagerDTO>> GetMyUserInfo()
    {
        var response = await managersService.GetManager(User.GetId());
        return response.ActionResult;
    }
}