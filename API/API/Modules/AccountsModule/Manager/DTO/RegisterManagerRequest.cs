using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API.Modules.AccountsModule.Manager.DTO;

public class RegisterManagerRequest
{
    public required string Login { get; set; }
    public required string Password { get; set; }
    [RegularExpression(@"^.+ .+ .+$")]
    [DefaultValue("Фамилия Имя Отчество")]
    public required string Fio { get; set; }
}