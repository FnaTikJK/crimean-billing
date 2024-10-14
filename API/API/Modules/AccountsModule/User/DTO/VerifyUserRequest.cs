namespace API.Modules.AccountsModule.User.DTO;

public class VerifyUserRequest
{
    public int verificationCode { get; set; }
    public string? PhoneNumber { get; set; }
}