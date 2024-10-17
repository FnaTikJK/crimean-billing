namespace API.Modules.ServicesModule;

public class ServicesModule : IModule
{
    public void RegisterModule(IServiceCollection services)
    {
        services.AddScoped<IServicesService, ServicesService>();
    }
}