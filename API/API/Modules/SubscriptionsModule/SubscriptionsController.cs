using API.Infrastructure.Extensions;
using API.Modules.AccountsModule.Share;
using API.Modules.AdminModule.DTO;
using API.Modules.SubscriptionsModule.DTO;
using API.Modules.SubscriptionsModule.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Modules.SubscriptionsModule;

[Authorize(Roles = nameof(AccountRole.User))]
[Route("api/[controller]")]
[ApiController]
public class SubscriptionsController : ControllerBase
{
    private readonly ISubscriptionsService subscriptionsService;

    public SubscriptionsController(ISubscriptionsService subscriptionsService)
    {
        this.subscriptionsService = subscriptionsService;
    }

    /// <summary>
    /// Создаёт/редачит подписку
    /// </summary>
    /// <remarks>
    /// Если указать другой TemplateId то подписка улетит в Preferred - то, на что она поменяется в момент PaymentPeriod
    /// <br/> Если указать текущий TemplateId, то Preferred очистится
    /// </remarks>
    [HttpPost("Subscribe")]
    public async Task<ActionResult<SubscriptionDTO>> Subscribe([FromBody] SubscribeRequest request)
    {
        var response = await subscriptionsService.Subscribe(request);
        return response.ActionResult;
    }

    /// <summary>
    /// Получить инфу о подписке
    /// </summary>
    /// <remarks>
    /// 404, если подписки нет
    /// </remarks>
    [HttpGet("")]
    public async Task<ActionResult<SubscriptionDTO>> GetMySubscription([FromQuery] GetSubscriptionRequest request)
    {
        var response = await subscriptionsService.GetMySubscription(request, User.GetId());
        return response.ActionResult;
    }

    /// <summary>
    /// Тратит Service в Tariff
    /// </summary>
    [HttpPost("Spend")]
    public async Task<ActionResult> SpendTariff(SpendSubscriptionRequest request)
    {
        var response = await subscriptionsService.SpendTariff(request);
        return response.ActionResult;
    }
}