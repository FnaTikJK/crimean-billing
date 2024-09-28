using Microsoft.AspNetCore.Mvc;

namespace API.Modules.LogsModule;

[Route("api/[controller]")]
[ApiController]
public class LogsController : ControllerBase
{
    private readonly ILogsService logsService;

    public LogsController(ILogsService logsService)
    {
        this.logsService = logsService;
    }

    /// <summary>
    /// Логи по текущему дню
    /// </summary>
    [HttpGet]
    public ActionResult GetTodayLogs() 
        => GetTodayLogs(DateOnly.FromDateTime(DateTime.UtcNow).ToString());

    /// <summary>
    /// Логи по конкретной дате
    /// </summary>
    /// <param name="date">Дата в формате yyyy-MM-dd</param>
    [HttpGet(@"{date:regex([[\d*]])}")]
    public ActionResult GetTodayLogs([FromRoute] string date)
    {
        if (!DateOnly.TryParse(date, out var dateOnly))
            return BadRequest("Неправильный формат в Route. Должен быть yyyy-MM-dd");
        
        throw new NotImplementedException();
    }
}