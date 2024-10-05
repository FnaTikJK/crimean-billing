namespace API.Modules.AccountsModule.User.DTO;

public class VerifyUserRequest
{
    public int verificationCode { get; set; }
    public int? PhoneNumber { get; set; }
}