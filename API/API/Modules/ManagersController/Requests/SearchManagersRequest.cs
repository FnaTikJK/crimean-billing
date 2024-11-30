using API.Infrastructure.BaseApiDTOs;

namespace API.Modules.ManagersController.Requests;

public class SearchManagersRequest : SearchRequest
{
    public string? Fio { get; set; }
}