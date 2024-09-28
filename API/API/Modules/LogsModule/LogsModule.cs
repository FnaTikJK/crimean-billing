namespace API.Modules.LogsModule;

public class LogsModule : IModule
{
    public void RegisterModule(IServiceCollection services)
    {
        services.AddScoped<ILogsService, LogsService>();
    }
}