namespace API.Modules.AccountsModule.User.DTO;

public class RegisterUserRequest
{
    public int PhoneNumber { get; set; }
    public int Number { get; set; }
    public string? Email { get; set; }
}