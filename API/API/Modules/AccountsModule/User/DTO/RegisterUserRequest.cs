using API.Modules.AccountsModule.Share;

namespace API.Modules.AccountsModule.User.DTO;

public class RegisterUserRequest
{
    public Guid? UserId { get; set; }
    public string PhoneNumber { get; set; }
    public int Number { get; set; }
    public string? Email { get; set; }
    public AccountType AccountType { get; set; }
}