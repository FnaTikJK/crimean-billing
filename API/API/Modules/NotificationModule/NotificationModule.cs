using API.Infrastructure.Config;
using API.Modules.TelegramModule;

namespace API.Modules.NotificationModule;

public class NotificationModule : IModule
{
    public void RegisterModule(IServiceCollection services)
    {
        if (Config.MailBoxLogin == null || Config.MailBoxPassword == null)
            services.AddScoped<IMailsService, MockedMailsService>();
        else
            services.AddScoped<IMailsService, MailsService>();
        
        services.AddSingleton<ITelegramService, TelegramService>();
        services.AddScoped<INotificationsService, NotificationsService>();
    }
}