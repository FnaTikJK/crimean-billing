namespace API.Modules.UserAccountsModule.Model.DTO;

public class UserInAccountDTO
{
    public required Guid UserId { get; set; }
    public required string Fio { get; set; }
}