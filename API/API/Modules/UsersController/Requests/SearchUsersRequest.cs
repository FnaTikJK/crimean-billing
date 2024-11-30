using API.Infrastructure.BaseApiDTOs;

namespace API.Modules.UsersController.Requests;

public class SearchUsersRequest : SearchRequest
{
    public string? Email { get; set; }
    public string? Fio { get; set; }
    public HashSet<Guid>? AccountIds { get; set; }
    public string? PhoneNumber { get; set; }
}