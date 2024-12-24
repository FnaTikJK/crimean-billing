using API.Modules.DatabaseModule.RandomFillingData;

namespace API.Modules.DatabaseModule;

public class DatabaseModule : IModule
{
    public void RegisterModule(IServiceCollection services)
    {
        services.AddScoped<IDatabaseService, DatabaseService>();
        services.AddScoped<IDatabaseServiceRandom, DatabaseServiceRandomFilling>();
    }
}