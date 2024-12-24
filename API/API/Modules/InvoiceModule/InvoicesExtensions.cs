using API.Modules.InvoiceModule.Model;

namespace API.Modules.InvoiceModule;

public static class InvoicesExtensions
{
    public static float CalculateTotalPrice(this InvoiceEntity invoice)
    {
        var tariffPrice = invoice.Tariff.Price;
        var servicesTotalPrice = invoice.ServiceUsages?.Sum(e => e.Service.Price)
                                 ?? 0;
        
        return tariffPrice + servicesTotalPrice;
    }
}