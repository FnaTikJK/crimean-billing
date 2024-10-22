namespace API.Modules.TelegramModule;

public class TelegramModule : IDaemonModule
{
    public void RegisterModule(IServiceCollection services)
    {
        services.AddSingleton<ITelegramDaemon, TelegramDaemon>();
    }

    public void ConfigureDaemons(WebApplication app)
    {
        app.Services.GetService(typeof(ITelegramDaemon));
    }
}