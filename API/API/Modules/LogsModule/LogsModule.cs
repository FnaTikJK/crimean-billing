using API.Infrastructure;

namespace API.Modules.LogsModule;

public class LogsModule : IModule
{
    public void RegisterModule(IServiceCollection services)
    {
        services.AddSingleton<ILog, Log>();
        services.AddScoped<ILogsService, LogsService>();
    }
}