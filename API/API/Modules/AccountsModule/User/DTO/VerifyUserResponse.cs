namespace API.Modules.AccountsModule.User.DTO;

public class VerifyUserResponse
{
    public Guid UserId { get; set; }
    public IEnumerable<Guid> AccountIds { get; set; }
}