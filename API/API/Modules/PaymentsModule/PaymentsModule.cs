namespace API.Modules.PaymentsModule;

public class PaymentsModule : IModule
{
    public void RegisterModule(IServiceCollection services)
    {
        services.AddScoped<IPaymentsService, PaymentsService>();
    }
}