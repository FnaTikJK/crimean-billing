using API.Modules.SubscriptionsModule.ServiceUsage;

namespace API.Modules.SubscriptionsModule;

public class SubscriptionsModule : IModule
{
    public void RegisterModule(IServiceCollection services)
    {
        services.AddScoped<ISubscriptionsService, SubscriptionsService>();
        services.AddScoped<IServiceUsageService, ServiceUsageService>();
    }
}