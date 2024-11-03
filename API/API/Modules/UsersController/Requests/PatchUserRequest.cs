using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API.Modules.UsersController.Requests;

public class PatchUserRequest
{
    [RegularExpression(@".+@.+\..+")]
    [DefaultValue("test@test.tt")]
    public string? Email { get; set; }
    public long? TelegramId { get; set; }
    [RegularExpression(@"^.+ .+ .+$")]
    [DefaultValue("Фамилия Имя Отчество")]
    public string? Fio { get; set; }
}