namespace API.Modules.AccountsModule.User.DTO;

public class VerifyUserRequest
{
    public string VerificationCode { get; set; }
    public string? PhoneNumber { get; set; }
}