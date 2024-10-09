namespace API.Modules.NotificationModule;

public class NotificationModule : IModule
{
    public void RegisterModule(IServiceCollection services)
    {
        services.AddScoped<INotificationService, NotificationService>();
    }
}