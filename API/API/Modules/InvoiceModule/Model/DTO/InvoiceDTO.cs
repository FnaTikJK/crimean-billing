namespace API.Modules.InvoiceModule.Model.DTO;

public class InvoiceDTO
{
    public required Guid Id { get; set; }
    public required float ToPay { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime? PayedAt { get; set; }
    public required Guid AccountId { get; set; }
}