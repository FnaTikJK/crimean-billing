using API.Modules.InvoiceModule.Model;

namespace API.Modules.InvoiceModule;

public static class InvoicesExtensions
{
    public static float CalculateTotalPrice(this InvoiceEntity invoice)
    {
        return invoice.Tariff.Price;
    }
}