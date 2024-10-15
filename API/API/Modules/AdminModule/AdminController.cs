using API.Modules.AdminModule.DTO;
using Microsoft.AspNetCore.Mvc;

namespace API.Modules.AdminModule;

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
}