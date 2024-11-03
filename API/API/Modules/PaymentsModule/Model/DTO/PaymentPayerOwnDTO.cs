using API.Modules.InvoiceModule.Model.DTO;

namespace API.Modules.PaymentsModule.Model.DTO;

public class PaymentPayerOwnDTO
{
    public Guid Id { get; set; }
    public required float Money { get; set; }
    public required PaymentType Type { get; set; }
    public required DateTime DateTime { get; set; }
    public InvoiceDTO? Invoice { get; set; }
}