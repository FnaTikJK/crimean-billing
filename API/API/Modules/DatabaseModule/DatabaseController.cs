using API.Infrastructure.Config;
using API.Modules.DatabaseModule.RandomFillingData;
using Microsoft.AspNetCore.Mvc;

namespace API.Modules.DatabaseModule;

[Route("api/[controller]")]
[ApiController]
public class DatabaseController : ControllerBase
{
    private readonly IDatabaseService databaseService;
    private readonly IDatabaseServiceRandom databaseServiceRandom;

    public DatabaseController(IDatabaseService databaseService, IDatabaseServiceRandom databaseServiceRandom)
    {
        this.databaseService = databaseService;
        this.databaseServiceRandom = databaseServiceRandom;
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
    
    /// <summary>
    /// Заполняет бд рандомными данными
    /// </summary>
    [HttpPost("RandomDatabase")]
    public async Task<ActionResult> RandomDatabase([FromQuery] Guid accountId)
    {
        var response =  await databaseServiceRandom.RecreateRandomDatabase(accountId);
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