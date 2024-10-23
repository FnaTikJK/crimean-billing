using API.Modules.AdminModule.DTO;
using Microsoft.AspNetCore.Mvc;

namespace API.Modules.AdminModule;

/// <summary>
/// Для всяких хаков сценариев
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly IAdminService adminService;

    public AdminController(IAdminService adminService)
    {
        this.adminService = adminService;
    }

    /// <summary>
    /// Получает код для номера телефона
    /// </summary>
    [HttpGet("VerificationCode")]
    public ActionResult<GetVerificationCodeResponse> GetVerificationCode([FromQuery]GetVerificationCodeRequest request)
    {
        var response = adminService.GetVerificationCode(request);
        return response.ActionResult;
    }

    /// <summary>
    /// Меняет время у программы на определённую дату (для демонов)
    /// </summary>
    [HttpPost("MockDateTime")]
    public ActionResult<DateTime> MockDatetime([FromQuery] MockDateTimeRequest request)
    {
        var dateTime = DateTime.Parse(request.Date);
        var response = adminService.MockDateTime(dateTime);
        return response.ActionResult;
    }
    
    /// <summary>
    /// Форсит вызов демона, создающего Invoice-ы
    /// </summary>
    /// <remarks>
    /// Проходит по подпискам и тем, у кого осталось 3 дня до PaymentPeriod создаёт Invoice
    /// <br/> Чтобы поменять дату - используй соседний контрол `MockDateTime`
    /// </remarks>
    [HttpPost("ForceInvoicesCreation")]
    public async Task<ActionResult> ForceInvoicesCreation()
    {
        var response = await adminService.ForceInvoicesCreation();
        return response.ActionResult;
    }
}