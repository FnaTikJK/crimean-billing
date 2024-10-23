namespace API.Modules.InvoiceModule;

public class InvoiceModule : IDaemonModule
{
    public void RegisterModule(IServiceCollection services)
    {
        services.AddScoped<IInvoicesService, InvoicesService>();
        services.AddScoped<IInvoicesDaemon, InvoicesDaemon>();
    }

    public void ConfigureDaemons(IServiceScope scope)
    {
        scope.ServiceProvider.GetRequiredService<IInvoicesDaemon>();
    }
}