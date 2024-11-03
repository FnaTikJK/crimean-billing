using API.Infrastructure.Config;

namespace API.Modules.NotificationModule;

public class NotificationModule : IModule
{
    public void RegisterModule(IServiceCollection services)
    {
        if (Config.MailBoxLogin == null || Config.MailBoxPassword == null)
            services.AddScoped<INotificationService, MockedNotificationService>();
        else
            services.AddScoped<INotificationService, NotificationService>();
    }
}