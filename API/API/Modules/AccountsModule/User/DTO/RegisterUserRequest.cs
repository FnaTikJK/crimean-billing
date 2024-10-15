using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using API.Modules.AccountsModule.Share;

namespace API.Modules.AccountsModule.User.DTO;

public class RegisterUserRequest
{
    public Guid? UserId { get; set; }
    [RegularExpression(@"8[0-9]{10}")]
    public string PhoneNumber { get; set; }
    public int Number { get; set; }
    [RegularExpression(@".+@.+\..+")]
    [DefaultValue("test@test.tt")]
    public string? Email { get; set; }
    public AccountType AccountType { get; set; }
}