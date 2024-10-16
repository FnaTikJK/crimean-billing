namespace API.Modules.AccountsModule.User.DTO;

public class RegisterAccountResponse
{
    public required Guid UserId { get; set; }
    public Guid AccountId { get; set; }
}