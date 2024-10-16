namespace API.Modules.ManagersController;

public class ManagersModule : IModule
{
    public void RegisterModule(IServiceCollection services)
    {
        services.AddScoped<IManagersService, ManagersService>();
    }
}