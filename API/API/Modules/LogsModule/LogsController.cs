using API.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace API.Modules.LogsModule;

[Route("api/[controller]")]
[ApiController]
public class LogsController : ControllerBase
{
    private readonly ILogsService logsService;
    private readonly ILog logger;

    public LogsController(ILogsService logsService, ILog logger)
    {
        this.logsService = logsService;
        this.logger = logger;
    }

    /// <summary>
    /// Логи по текущему дню
    /// </summary>
    [HttpGet]
    public ActionResult<string> GetTodayLogs() 
        => GetTodayLogs(DateOnly.FromDateTime(DateTime.UtcNow).ToString());

    /// <summary>
    /// Логи по конкретной дате
    /// </summary>
    /// <param name="date">Дата в формате yyyy-MM-dd</param>
    [HttpGet(@"{date:regex([[\d*]])}")]
    public ActionResult<string> GetTodayLogs([FromRoute] string date)
    {
        if (!DateOnly.TryParse(date, out var dateOnly))
            return BadRequest("Неправильный формат в Route. Должен быть yyyy-MM-dd");

        return logsService.ReadLog(dateOnly);
    }
    
    [HttpGet("throw")]
    public ActionResult Throw()
    {
        try
        {
            throw new Exception("Тестовое исключение");
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            return StatusCode(500, "Произошла ошибка");
        }
    }
}