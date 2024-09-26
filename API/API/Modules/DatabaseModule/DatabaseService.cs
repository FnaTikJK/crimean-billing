using API.DAL;
using API.Infrastructure;

namespace API.Modules.DatabaseModule;

public interface IDatabaseService
{
    Result<bool> RecreateDatabase();
}

public class DatabaseService : IDatabaseService
{
    private readonly DataContext dataContext;

    public DatabaseService(DataContext dataContext)
    {
        this.dataContext = dataContext;
    }
    
    public Result<bool> RecreateDatabase()
    {
        dataContext.RecreateDatabase();

        return Result.NoContent<bool>();
    }
}