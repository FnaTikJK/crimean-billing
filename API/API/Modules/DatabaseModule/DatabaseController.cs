using API.DAL;
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
    public ActionResult RecreateDatabase()
    {
        return databaseService.RecreateDatabase().ActionResult;
    }
}