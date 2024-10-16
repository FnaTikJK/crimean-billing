namespace API.Modules.AdminModule;

public class AdminModule : IModule
{
    public void RegisterModule(IServiceCollection services)
    {
        services.AddScoped<IAdminService, AdminService>();
    }
}