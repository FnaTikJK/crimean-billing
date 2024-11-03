using API.Modules.InvoiceModule.Model;
using API.Modules.InvoiceModule.Model.DTO;

namespace API.Modules.InvoiceModule;

public static class InvoicesMapper
{
    public static InvoiceDTO Map(InvoiceEntity invoice)
    {
        return new InvoiceDTO
        {
            Id = invoice.Id,
            CreatedAt = invoice.CreatedAt,
            PayedAt = invoice.PayedAt,
            ToPay = invoice.CalculateTotalPrice(),
            AccountId = invoice.AccountId,
        };
    }
}