namespace API.Modules.AccountsModule.User.DTO;

public class RegisterUserResponse
{
    public required Guid UserId { get; set; }
    public Guid AccountId { get; set; }
}