using API.Infrastructure.Extensions;
using API.Modules.AccountsModule.Share;
using API.Modules.ManagersController.DTO;
using API.Modules.ManagersController.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Modules.ManagersController;

[Authorize(Roles = nameof(AccountRole.Manager))]
[Route("api/[controller]")]
[ApiController]
public class ManagersController : ControllerBase
{
    private readonly IManagersService managersService;

    public ManagersController(IManagersService managersService)
    {
        this.managersService = managersService;
    }

    [HttpGet("My")]
    public async Task<ActionResult<ManagerDTO>> GetMyUserInfo()
    {
        var response = await managersService.GetManager(User.GetId());
        return response.ActionResult;
    }
    
    /// <summary>
    /// Поиск по менеджерам
    /// </summary>
    [HttpPost("Search")]
    public ActionResult<SearchManagersResponse> Search(SearchManagersRequest request)
    {
        var response = managersService.Search(request);
        return response.ActionResult;
    }
}