namespace API.Modules.SubscriptionsModule;

public class SubscriptionsModule : IModule
{
    public void RegisterModule(IServiceCollection services)
    {
        services.AddScoped<ISubscriptionsService, SubscriptionsService>();
    }
}