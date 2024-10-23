using System.ComponentModel;

namespace API.Modules.AccountsModule.User.DTO;

public class LoginUserRequest
{
    [DefaultValue("88005553535")]
    public string PhoneNumber { get; set; }
}