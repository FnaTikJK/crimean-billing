using System.ComponentModel.DataAnnotations;

namespace API.Modules.UsersController.Requests;

public class PatchUserRequest
{
    [RegularExpression(@".+@.+\..+")]
    public string? Email { get; set; }
    public long? TelegramId { get; set; }
    [RegularExpression(@"^.+ .+ .+$")]
    public string? Fio { get; set; }
}