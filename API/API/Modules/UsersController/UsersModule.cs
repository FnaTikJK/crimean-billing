namespace API.Modules.UsersController;

public class UsersModule : IModule
{
    public void RegisterModule(IServiceCollection services)
    {
        services.AddScoped<IUsersService, UsersService>();
    }
}