using API.Infrastructure.BaseApiDTOs;
using API.Modules.PaymentsModule.Model;

namespace API.Modules.PaymentsModule.DTO;

public class SearchPaymentsRequest : SearchRequestWithoutIds
{
    public Guid AccountId { get; set; }
    public PaymentType? PaymentType { get; set; }
}