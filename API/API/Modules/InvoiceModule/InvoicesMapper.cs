using API.Modules.InvoiceModule.Model;
using API.Modules.InvoiceModule.Model.DTO;

namespace API.Modules.InvoiceModule;

public static class InvoicesMapper
{
    public static InvoiceDTO Map(InvoiceEntity invoice)
    {
        return new InvoiceDTO()
        {
            Id = invoice.Id,
            CreatedAt = invoice.CreatedAt,
            PayedAt = invoice.PayedAt,
            ToPay = CalculatePrice(invoice)
        };
    }

    private static float CalculatePrice(InvoiceEntity invoice)
    {
        return invoice.Tariff.Price;
    }
}