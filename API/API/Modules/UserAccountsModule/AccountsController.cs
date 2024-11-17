using API.Modules.UserAccountsModule.DTO;
using API.Modules.UsersController.DTO;
using Microsoft.AspNetCore.Mvc;

namespace API.Modules.UserAccountsModule;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IAccountsService accountsService;

    public AccountsController(IAccountsService accountsService)
    {
        this.accountsService = accountsService;
    }

    [HttpPost("Search")]
    public ActionResult<SearchAccountsResponse> Search(SearchAccountsRequest request)
    {
        var response = accountsService.Search(request);
        return response.ActionResult;
    }

    [HttpPatch("{accountId:Guid}")]
    public async Task<ActionResult<AccountDTO>> PatchAccount([FromRoute] Guid accountId, PatchAccountRequest request)
    {
        var response = await accountsService.PatchAccount(accountId, request);
        return response.ActionResult;
    }
}