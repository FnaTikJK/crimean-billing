namespace API.Modules.AccountsModule.User.DTO;

public class RegisterUserRequest
{
    public Guid? UserId { get; set; }
    public int PhoneNumber { get; set; }
}