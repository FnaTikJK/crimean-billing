using API.Modules.InvoiceModule.Model;
using API.Modules.InvoiceModule.Model.DTO;
using API.Modules.PaymentsModule.Model;

namespace API.Modules.InvoiceModule;

public static class InvoicesMapper
{
    public static InvoiceDTO Map(InvoiceEntity invoice)
    {
        return new InvoiceDTO
        {
            Id = invoice.Id,
            CreatedAt = invoice.CreatedAt,
            PayedAt = invoice.GetPayedAt(),
            ToPay = invoice.CalculateTotalPrice(),
            AccountId = invoice.AccountId,
        };
    }

    public static InvoiceDTO? Map(InvoiceEntity invoice, PaymentEntity payment)
    {
        if (payment.InvoiceId != invoice.Id)
            throw new ArgumentException("Incorrect Invoice mapping. Payment is not reffered to invoice");

        return new InvoiceDTO()
        {
            Id = invoice.Id,
            AccountId = invoice.AccountId,
            CreatedAt = invoice.CreatedAt,
            ToPay = payment.Money,
            PayedAt = payment.DateTime,
        };
    }
}