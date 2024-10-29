using System.ComponentModel;
using API.Infrastructure.BaseApiDTOs;

namespace API.Modules.InvoiceModule.DTO;

public class SearchInvoicesRequest : SearchRequestWithoutIds
{
    public required Guid AccountId { get; set; }
    [DefaultValue(null)]
    public bool? IsPayed { get; set; }
}