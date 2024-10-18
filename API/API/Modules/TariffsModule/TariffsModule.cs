namespace API.Modules.TariffsModule;

public class TariffsModule : IModule
{
    public void RegisterModule(IServiceCollection services)
    {
        services.AddScoped<ITariffsService, TariffsService>();
    }
}