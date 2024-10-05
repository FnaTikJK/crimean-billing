namespace API.Modules.AccountsModule;

public class AuthModule : IModule
{
    public void RegisterModule(IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
    }
}