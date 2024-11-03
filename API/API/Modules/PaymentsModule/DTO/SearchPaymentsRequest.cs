using API.Infrastructure.BaseApiDTOs;

namespace API.Modules.PaymentsModule.DTO;

public class SearchPaymentsRequest : SearchRequestWithoutIds
{
    public Guid AccountId { get; set; }
}