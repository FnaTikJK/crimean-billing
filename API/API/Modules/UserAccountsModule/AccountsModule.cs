namespace API.Modules.UserAccountsModule;

public class AccountsModule : IModule
{
    public void RegisterModule(IServiceCollection services)
    {
        services.AddScoped<IAccountsService, AccountsService>();
    }
}