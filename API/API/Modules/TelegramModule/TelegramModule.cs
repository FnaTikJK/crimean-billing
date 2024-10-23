namespace API.Modules.TelegramModule;

public class TelegramModule : IDaemonModule
{
    public void RegisterModule(IServiceCollection services)
    {
        services.AddSingleton<ITelegramDaemon, TelegramDaemon>();
    }

    public void ConfigureDaemons(IServiceScope scope)
    {
        scope.ServiceProvider.GetRequiredService<ITelegramDaemon>();
    }
}