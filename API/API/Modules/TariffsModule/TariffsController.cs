using API.Modules.AccountsModule.Share;
using API.Modules.TariffsModule.DTO;
using API.Modules.TariffsModule.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Modules.TariffsModule;

[Authorize(Roles = nameof(AccountRole.Manager))]
[Route("api/[controller]")]
[ApiController]
public class TariffsController : ControllerBase
{
    private readonly ITariffsService tariffsService;

    public TariffsController(ITariffsService tariffsService)
    {
        this.tariffsService = tariffsService;
    }

    [HttpPost("")]
    public async Task<ActionResult<TariffDTO>> CreateTariff([FromBody] CreateTariffRequest request)
    {
        var response = await tariffsService.CreateTariff(request);
        return response.ActionResult;
    }
}