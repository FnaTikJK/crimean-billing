using API.Infrastructure;
using ILogger = Serilog.ILogger;

namespace API.Modules.LogsModule;

public class LogsModule : IModule
{
    public void RegisterModule(IServiceCollection services)
    {
        services.AddSingleton<ILog, Log>();
        services.AddScoped<ILogsService, LogsService>();
        services.AddScoped<ILog, Log>();
    }
}