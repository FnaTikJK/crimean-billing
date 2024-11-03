using API.Modules.PaymentsModule.DTO;
using Microsoft.AspNetCore.Mvc;

namespace API.Modules.PaymentsModule;

/// <summary>
/// Сервис, отвечающий за финансы
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentsService paymentsService;

    public PaymentsController(IPaymentsService paymentsService)
    {
        this.paymentsService = paymentsService;
    }

    /// <summary>
    /// История операций с деньгами.
    /// </summary>
    /// <remarks>
    /// Type: {0: ополнение, 1: трата}
    /// <br/>Сортировка по убыванию DateTime.
    /// </remarks>
    [HttpPost("Search")]
    public ActionResult<SearchPaymentsResponse> Search(SearchPaymentsRequest request)
    {
        var response = paymentsService.Search(request);
        return response.ActionResult;
    }

    /// <summary>
    /// Добавит пользователю сколько укажешь
    /// </summary>
    [HttpPost("Add")]
    public async Task<ActionResult<PaymentsResponse>> AddMoney(AddMoneyRequest request)
    {
        var response = await paymentsService.AddMoney(request);
        return response.ActionResult;
    }
    
    /// <summary>
    /// Снимет со счёта сколько укажешь.
    /// Ошибка, если недосаточно денег
    /// </summary>
    [HttpPost("Spend")]
    public async Task<ActionResult<PaymentsResponse>> SpendMoney(SpendMoneyRequest request)
    {
        var response = await paymentsService.SpendMoney(request);
        return response.ActionResult;
    }
}