using API.Modules.InvoiceModule;
using API.Modules.PaymentsModule.Model;
using API.Modules.PaymentsModule.Model.DTO;

namespace API.Modules.PaymentsModule;

public static class PaymentsMapper
{
    public static PaymentPayerOwnDTO Map(PaymentEntity payment)
    {
        return new PaymentPayerOwnDTO
        {
            Id = payment.Id,
            Money = payment.Money,
            Type = payment.Type,
            DateTime = payment.DateTime,
            Invoice = payment.Invoice != null
                ? InvoicesMapper.Map(payment.Invoice, payment)
                : null,
        };
    }
}