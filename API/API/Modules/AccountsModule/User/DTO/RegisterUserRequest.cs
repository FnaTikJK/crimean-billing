using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API.Modules.AccountsModule.User.DTO;

public class RegisterUserRequest
{
    [RegularExpression(@".+@.+\..+")]
    [DefaultValue("test@test.tt")]
    public required string Email { get; set; }
    [RegularExpression(@"^.+ .+ .+$")]
    [DefaultValue("Фамилия Имя Отчество")]
    public required string Fio { get; set; }
}