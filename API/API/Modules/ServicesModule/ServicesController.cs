using API.Modules.AccountsModule.Share;
using API.Modules.ServicesModule.DTO;
using API.Modules.ServicesModule.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Modules.ServicesModule;

[Authorize(Roles = nameof(AccountRole.Manager))]
[Route("api/[controller]")]
[ApiController]
public class ServicesController : ControllerBase
{
    private readonly IServicesService servicesService;

    public ServicesController(IServicesService servicesService)
    {
        this.servicesService = servicesService;
    }

    /// <summary>
    /// Добавляет Service 
    /// </summary>
    /// <remarks>
    /// Возвращает плоскую модель(поля в БД лежат по-другому, см. БД)
    /// </remarks>
    [HttpPost("")]
    public async Task<ActionResult<ServiceDTO>> CreateService([FromBody] CreateServiceRequest request)
    {
        var response = await servicesService.CreateService(request);
        return response.ActionResult;
    }
    
    /// <summary>
    /// Редактирует Service 
    /// </summary>
    /// <remarks>
    /// Ищет по одному из Id. Для Template выбирает актуальный(DeletedAt != null) Service.
    /// <br/>Редактируются только !null поля.
    /// <br/>Если отредачить часть Service (Price, Amount), то у него сменится Id, тк мы создали новый, чтобы сохранить стат. данные
    /// <br/><strong>NeedKillService</strong> - флаг, чтобы оставить только шаблон
    /// </remarks>>
    [HttpPatch("")]
    public async Task<ActionResult<ServiceDTO>> PatchService([FromBody] PatchServiceRequest request)
    {
        var response = await servicesService.PatchService(request);
        return response.ActionResult;
    }

    /// <summary>
    /// Поиск по Services
    /// </summary>
    /// <remarks>
    /// OrderBy: {Code,Price,Amount}
    /// <br/> OrderDirection: {Desc, Asc}. 
    /// </remarks>
    [HttpPost("Search")]
    public async Task<ActionResult<SearchServicesResponse>> Search([FromBody] SearchServicesRequest request)
    {
        var response = await servicesService.SearchServices(request);
        return response.ActionResult;
    }
    
    /// <summary>
    /// Получить полную инфу о сервисе с историей изменений
    /// </summary>
    [HttpGet("{templateId:Guid}")]
    public async Task<ActionResult<ServiceWithHistoryDTO>> GetServiceInfo([FromRoute] Guid templateId)
    {
        var response = await servicesService.GetServiceInfo(templateId);
        return response.ActionResult;
    }
}