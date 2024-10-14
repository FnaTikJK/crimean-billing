namespace API.Modules.TelegramModule;

public class TelegramModule : IModule
{
    public void RegisterModule(IServiceCollection services)
    {
        services.AddSingleton<ITelegramService, TelegramService>();
    }
}