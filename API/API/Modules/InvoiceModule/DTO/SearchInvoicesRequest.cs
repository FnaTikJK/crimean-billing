using API.Infrastructure.BaseApiDTOs;

namespace API.Modules.InvoiceModule.DTO;

public class SearchInvoicesRequest : SearchRequestWithoutIds
{
    public required Guid AccountId { get; set; }
}