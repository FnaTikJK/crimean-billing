using API.Infrastructure.Extensions;
using API.Modules.AccountsModule.Share;
using API.Modules.AdminModule.DTO;
using API.Modules.SubscriptionsModule.DTO;
using API.Modules.SubscriptionsModule.Model.DTO;
using API.Modules.SubscriptionsModule.ServiceUsage;
using API.Modules.SubscriptionsModule.ServiceUsage.DTO;
using API.Modules.SubscriptionsModule.ServiceUsage.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Modules.SubscriptionsModule;

[Authorize(Roles = nameof(AccountRole.User))]
[Route("api/[controller]")]
[ApiController]
public class SubscriptionsController : ControllerBase
{
    private readonly ISubscriptionsService subscriptionsService;
    private readonly IServiceUsageService serviceUsageService;

    public SubscriptionsController(ISubscriptionsService subscriptionsService, IServiceUsageService serviceUsageService)
    {
        this.subscriptionsService = subscriptionsService;
        this.serviceUsageService = serviceUsageService;
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

    /// <summary>
    /// Добавляет в подписку доп. услугу. Сбрасывается в PaymentDate
    /// </summary>
    [HttpPost("Services")]
    public async Task<ActionResult<ServiceUsageDTO>> AddService([FromBody] AddServiceRequest request)
    {
        var response = await serviceUsageService.AddService(request);
        return response.ActionResult;
    }

    /// <summary>
    /// Тратит доп. услугу
    /// </summary>
    [HttpPost("Services/Spend")]
    public async Task<ActionResult<ServiceUsageDTO>> SpendService([FromBody] SpendServiceRequest request)
    {
        var response = await serviceUsageService.SpendService(request);
        return response.ActionResult;
    }
}