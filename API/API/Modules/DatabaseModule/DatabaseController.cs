using API.Infrastructure.Config;
using Microsoft.AspNetCore.Mvc;

namespace API.Modules.DatabaseModule;

[Route("api/[controller]")]
[ApiController]
public class DatabaseController : ControllerBase
{
    private readonly IDatabaseService databaseService;

    public DatabaseController(IDatabaseService databaseService)
    {
        this.databaseService = databaseService;
    }

    /// <summary>
    /// Пересоздаёт БД
    /// </summary>
    [HttpPost]
    public async Task<ActionResult> RecreateDatabase([FromQuery] bool withAutoFilling = true)
    {
        var response =  await databaseService.RecreateDatabase(withAutoFilling);
        return response.ActionResult;
    }

    [HttpGet]
    public ActionResult GetConnectionstring()
    {
        return Ok(new {
            Connection = Config.DatabaseConnectionString,
            IsDebug = Config.IsDebug,
        });
    }

    [HttpGet("Config")]
    public ActionResult GetConfig()
    {
        return Ok(new
        {
            Config.IsDebug,
            Config.DatabaseConnectionString,
            Config.MailBoxLogin,
            Config.MailBoxPassword,
        });
    }
}