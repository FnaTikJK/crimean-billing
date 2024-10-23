namespace API.Modules.PaymentsModule;

public class PaymentsModule : IDaemonModule
{
    public void RegisterModule(IServiceCollection services)
    {
        services.AddScoped<IPaymentsService, PaymentsService>();
        services.AddScoped<IPaymentsDaemon, PaymentsDaemon>();
    }

    public void ConfigureDaemons(IServiceScope scope)
    {
        scope.ServiceProvider.GetRequiredService<IPaymentsDaemon>();
    }
}